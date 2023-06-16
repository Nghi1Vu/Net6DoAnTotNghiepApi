using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class PostOneDoorCommand : IRequest<string>
{
    public PostOneDoor model { get; set; }
}
