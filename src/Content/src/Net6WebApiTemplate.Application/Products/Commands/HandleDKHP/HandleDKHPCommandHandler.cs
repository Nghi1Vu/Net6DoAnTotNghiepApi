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
            //List<ProgramSemester> CHEKmd = _productRepository.GetProgramSemester(request.CourseIndustryID, request.CourseID, request.UserID);
            if ((kQHTs != null && kQHTs.Where(x => x.ModulesID == request.mdid).Count() > 0) /*|| (CHEKmd != null && CHEKmd.Where(x => x.ModulesID == request.mdid && (x.ScoreFinal != null || x.D4 != null ||x.Score1!=null||x.XH!=null)).Count()>0)*/)
            {
                return "N|Đã đăng ký hoặc đã học học phần này trước đó";
            }
            ExamByClass byClasses = _productRepository.GetExamByClass(request.id).FirstOrDefault();
            List<DKHPByTKB> tKB = _productRepository.GetDKHPByTKB(request.UserID);
            int modul = byClasses.ModulesID;
            List<IndependentClass> classes = _productRepository.GetIC(modul,request.CourseID,request.CourseIndustryID);
            List<ProgramSemester> programs = _productRepository.GetProgramSemester(request.CourseIndustryID,request.CourseID,request.UserID);
            if (programs.Count > 0)
            {
                string[] mdht = !string.IsNullOrEmpty(programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesHT)? programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesHT.Split(','):new string[0];
                string[] mdtq = !string.IsNullOrEmpty(programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesTQ)? programs.Where(x => x.ModulesID == request.mdid).FirstOrDefault().ModulesTQ.Split(',') : new string[0] ;
                if (mdht.Length > 0)
                {
                    for (int i = 0; i < mdht.Length; i++)
                    {
                        if (exams != null && exams.Where(x => x.ModulesCode == mdht[i].Trim() && x.ScoreFinal > 4).Count() <= 0)
                        {
                            return "N|Cần học học phần học trước của học phần này";
                        }
                    }
                }
                if (mdtq.Length > 0)
                {
                    for (int i = 0; i < mdtq.Length; i++)
                    {
                        if (exams != null && exams.Where(x => x.ModulesID == int.Parse(mdtq[i].Trim()) && x.ScoreFinal > 4).Count() <= 0)
                        {
                            return "N|Cần học hết các học phần tiên quyết của học phần này";
                        }
                    }
                }
                
            }
            if (classes != null)
            {
                var cls = classes.Where(x => x.IndependentClassID == request.id).FirstOrDefault();
                if(tKB.Where(x=>x.TimesInDay==cls.TimesInDay && x.DayStudy==cls.DayStudy && (x.timeday.Contains(cls.timeday) || cls.timeday.Contains(x.timeday))).Count() > 0)
                {
                    return "N|Trùng lịch học";
                }

                if ((int.Parse(cls.SSSV.Split('/')[1])- int.Parse(cls.SSSV.Split('/')[0])) <= 0)
                {
                    return "N|Đã đủ số lượng sinh viên đăng ký"; 
                }
                //money = request.amount - cls.Amount;
                //if (money <= 0)
                //{
                //    return "N|Số dư không đủ";
                //}
                kq = _productRepository.PostIC(request.UserID, request.id, cls.Amount, request.mdid,money);
               
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
