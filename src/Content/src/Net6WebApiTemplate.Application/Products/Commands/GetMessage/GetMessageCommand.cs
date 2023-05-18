using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetMessageCommand : IRequest<List<Message>>
{
    public int ClassID { get; set; }
}
