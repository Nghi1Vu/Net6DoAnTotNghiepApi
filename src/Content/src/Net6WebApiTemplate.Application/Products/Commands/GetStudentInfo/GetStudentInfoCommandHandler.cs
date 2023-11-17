using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetStudentInfoCommandHandler : IRequestHandler<GetStudentInfoCommand, StudentInfo>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetStudentInfoCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<StudentInfo> Handle(GetStudentInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            StudentInfo studentInfos = _productRepository.GetStudentInfo(request.Username, request.Password);
            return studentInfos;
        }
        catch(Exception ex) 
        {
            throw new UnauthorizedException(ex.Message);
        }
    }
}
