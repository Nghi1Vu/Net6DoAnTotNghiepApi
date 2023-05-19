using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTTCNCommand : IRequest<List<TTCN>>
{
    public int UserID { get; set; }
}
