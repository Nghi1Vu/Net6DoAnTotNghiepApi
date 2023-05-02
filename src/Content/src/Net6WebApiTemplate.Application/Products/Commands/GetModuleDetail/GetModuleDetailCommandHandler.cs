using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetModuleDetailCommandHandler : IRequestHandler<GetModuleDetailCommand, List<ModuleDetail>>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetModuleDetailCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<List<ModuleDetail>> Handle(GetModuleDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<ModuleDetail> result = _productRepository.GetModuleDetail(request.ModulesID);
            return result;
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
