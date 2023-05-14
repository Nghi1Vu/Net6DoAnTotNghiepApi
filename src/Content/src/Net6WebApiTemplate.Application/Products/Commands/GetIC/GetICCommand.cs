using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetICCommand : IRequest<List<IndependentClass>>
{
    public int ModulesID { get; set; }
}
