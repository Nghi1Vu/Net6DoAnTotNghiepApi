using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetStudentClassCommandHandler : IRequestHandler<GetStudentClassCommand, List<StudenClass>>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetStudentClassCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<List<StudenClass>> Handle(GetStudentClassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<StudenClass> news = _productRepository.GetStudentClass(request.ClassID);
            return news;
        }
        catch
        {
            throw new UnauthorizedException("Invalid student.");
        }
    }
}
