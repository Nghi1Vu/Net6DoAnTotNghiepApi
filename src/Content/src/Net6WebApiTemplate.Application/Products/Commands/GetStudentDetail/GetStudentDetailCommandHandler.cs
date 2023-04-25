using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetStudentDetailCommandHandler : IRequestHandler<GetStudentDetailCommand, StudentDetail>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetStudentDetailCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<StudentDetail> Handle(GetStudentDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            StudentDetail studentInfos = _productRepository.GetStudentDetail(request.UserId);
            return studentInfos;
        }
        catch
        {
            throw new UnauthorizedException("Invalid student.");
        }
    }
}
