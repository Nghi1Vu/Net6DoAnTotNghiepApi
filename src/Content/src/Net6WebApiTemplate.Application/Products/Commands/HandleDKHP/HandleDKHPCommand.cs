using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class HandleDKHPCommand : IRequest<string>
{
    public int id { get; set; }
    public int UserID { get; set; }
    public int mdid { get; set; }
    public decimal amount { get; set; }
}
