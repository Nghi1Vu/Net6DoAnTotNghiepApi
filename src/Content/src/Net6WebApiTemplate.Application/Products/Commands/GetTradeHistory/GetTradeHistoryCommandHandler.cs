using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetTradeHistoryCommandHandler : IRequestHandler<GetTradeHistoryCommand, List<TradeHistory>>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetTradeHistoryCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<List<TradeHistory>> Handle(GetTradeHistoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<TradeHistory> result = _productRepository.GetTradeHistory(request.UserID);
            return result;
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
