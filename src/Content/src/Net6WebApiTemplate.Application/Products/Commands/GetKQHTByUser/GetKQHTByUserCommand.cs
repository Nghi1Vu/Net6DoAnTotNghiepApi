using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetKQHTByUserCommand : IRequest<List<KQHT>>
{
    public int UserID { get; set; }
}
