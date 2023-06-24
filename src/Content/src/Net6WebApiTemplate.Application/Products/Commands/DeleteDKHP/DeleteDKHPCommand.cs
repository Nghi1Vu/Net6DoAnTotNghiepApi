using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class DeleteDKHPCommand : IRequest<string>
{
    public int UserID { get; set; }
    public int IndependentClassID { get; set; }
}
