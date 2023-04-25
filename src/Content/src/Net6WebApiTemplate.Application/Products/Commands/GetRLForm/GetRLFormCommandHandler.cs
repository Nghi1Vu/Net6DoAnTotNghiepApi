using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetRLFormCommandHandler : IRequestHandler<GetRLFormCommand, RLForm>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetRLFormCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<RLForm> Handle(GetRLFormCommand request, CancellationToken cancellationToken)
    {
        try
        {
            RLForm studentInfos = _productRepository.GetRLForm(request.UserId);
            return studentInfos;
        }
        catch
        {
            throw new UnauthorizedException("Invalid student.");
        }
    }
}
