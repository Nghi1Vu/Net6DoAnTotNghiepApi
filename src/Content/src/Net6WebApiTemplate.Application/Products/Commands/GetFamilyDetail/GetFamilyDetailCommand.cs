using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetFamilyDetailCommand : IRequest<FamilyDetail>
{
    public int UserId { get; set; }
}
