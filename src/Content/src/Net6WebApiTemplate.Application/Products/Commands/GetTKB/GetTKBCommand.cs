using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTKBCommand : IRequest<List<TKB>>
{
    public int UserID { get; set; }
    public string aDate { get; set; }
    public string eDate { get; set; }
}
