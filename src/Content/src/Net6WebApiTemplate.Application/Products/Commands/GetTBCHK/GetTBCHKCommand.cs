using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTBCHKCommand : IRequest<List<TBCHKModel>>
{
    public int UserID { get; set; }
}
