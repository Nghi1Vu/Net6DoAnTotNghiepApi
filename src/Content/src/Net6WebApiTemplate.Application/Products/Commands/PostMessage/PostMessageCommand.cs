using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class PostMessageCommand : IRequest<string>
{
    public int UserID { get; set; }
    public string content { get; set; }
}
