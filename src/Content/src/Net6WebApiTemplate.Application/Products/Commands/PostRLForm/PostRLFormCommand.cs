using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class PostRLFormCommand : IRequest<int>
{
    public List<PostRLForm> model { get; set; }
}
