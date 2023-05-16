using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetDsGtHsCommand : IRequest<List<DsGtHs>>
{
    public int UserID { get; set; }
}
