using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class HandleDKHPCommandHandler : IRequestHandler<HandleDKHPCommand, string>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public HandleDKHPCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<string> Handle(HandleDKHPCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string kq = "";
            decimal money = 0;
            List<KQHT> kQHTs = _productRepository.GetKQHTByUser(request.UserID);
            List<ExamResult> exams = _productRepository.GetExamResult(request.UserID);
            if(kQHTs != null && kQHTs.Where(x => x.ModulesID == request.mdid).Count() > 0)
            {
                return "N|Đã học học phần này trước đó";
            }
            ExamByClass byClasses = _productRepository.GetExamByClass(request.id).FirstOrDefault();
            List<TKB> tKB = _productRepository.GetTKB(request.UserID, "", "");
            int modul = byClasses.ModulesID;
            List<IndependentClass> classes = _productRepository.GetIC(modul);
            List<ProgramSemester> programs = _productRepository.GetProgramSemester();
            if (programs.Count > 0)
            {
                string[] mdht = programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesHT.Split(',');
                string[] mdtq = programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesTQ.Split(',');
                for(int i = 0; i < mdht.Length; i++)
                {
                    if (exams != null && exams.Where(x => x.ModulesID == int.Parse(mdht[i].Trim())&& x.ScoreFinal>4).Count() <= 0)
                    {
                        return "N|Cần học học phần học trước của học phần này";
                    }
                }
                for (int i = 0; i < mdtq.Length; i++)
                {
                    if (exams != null && exams.Where(x => x.ModulesID == int.Parse(mdtq[i].Trim()) && x.ScoreFinal > 4).Count() <= 0)
                    {
                        return "N|Cần học hết các học phần tiên quyết của học phần này";
                    }
                }
            }
            if (classes != null)
            {
                var cls = classes.Where(x => x.IndependentClassID == request.id).FirstOrDefault();
                if(tKB.Where(x=>x.termid==19&&x.TimesInDay==cls.TimesInDay && x.DayStudy==cls.DayStudy && (x.StudyTime.Contains(cls.timeday) || cls.timeday.Contains(x.StudyTime))).Count() > 0)
                {
                    return "N|Trùng lịch học";
                }

                if ((int.Parse(cls.SSSV.Split('/')[1])- int.Parse(cls.SSSV.Split('/')[0])) <= 0)
                {
                    return "N|Đã đủ số lượng sinh viên đăng ký"; 
                }
                money = request.amount - cls.Amount;
                kq = _productRepository.PostIC(request.UserID, request.id, cls.Amount, request.mdid);
               
            }
            if (money <= 0)
            {
                return "N|Số dư không đủ"; 
            }
            if (kq == "Y")
            {
                return "Y|Thành công";

            }
            else
            {
                return "N|Thất bại";
            }
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
