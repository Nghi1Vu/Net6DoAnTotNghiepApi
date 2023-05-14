using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetKQHTByClassCommand : IRequest<List<KQHT>>
{
    public int IndependentClassID { get; set; }
}
