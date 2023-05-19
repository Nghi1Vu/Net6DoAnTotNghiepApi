using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetChannelAmountCommand : IRequest<List<ChannelAmount>>
{
    public int ClassID { get; set; }
}
