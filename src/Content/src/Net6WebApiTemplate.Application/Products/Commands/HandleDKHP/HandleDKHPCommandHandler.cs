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
            decimal money = 0;
            List<KQHT> kQHTs = _productRepository.GetKQHTByUser(request.UserID);
            if(kQHTs != null && kQHTs.Where(x => x.IndependentClassID == request.id).Count() > 0)
            {
                return "N|Đã đăng ký hoặc đã học học phần này trước đó";
            }
            ExamByClass byClasses = _productRepository.GetExamByClass(request.id).FirstOrDefault();
            int modul = byClasses.ModulesID;
            List<IndependentClass> classes = _productRepository.GetIC(modul);
            if (classes != null)
            {
                var cls = classes.Where(x => x.IndependentClassID == request.id).FirstOrDefault();
                if ((int.Parse(cls.SSSV.Split('/')[1])- int.Parse(cls.SSSV.Split('/')[0])) <= 0)
                {
                    return "N|Đã đủ số lượng sinh viên đăng ký"; 
                }
                money = request.amount - cls.Amount;
            }
            if (money <= 0)
            {
                return "N|Số dư không đủ"; 
            }
            return "Y|Thành công";
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
