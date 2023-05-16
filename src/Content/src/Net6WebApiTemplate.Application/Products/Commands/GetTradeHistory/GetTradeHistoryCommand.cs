using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTradeHistoryCommand : IRequest<List<TradeHistory>>
{
    public int UserID { get; set; }
}
