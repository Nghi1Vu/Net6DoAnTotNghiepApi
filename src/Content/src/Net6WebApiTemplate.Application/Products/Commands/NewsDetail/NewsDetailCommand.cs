using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class NewsDetailCommand : IRequest<News>
{
    public int NewsId { get; set; }
}
