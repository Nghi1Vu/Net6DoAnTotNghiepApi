using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTTCNDoneCommand : IRequest<List<TTCN>>
{
    public int UserID { get; set; }
}
