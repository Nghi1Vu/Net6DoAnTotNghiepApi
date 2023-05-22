using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTeachCalendarDetailCommand : IRequest<List<TeachCalendarDetail>>
{
    public int IndependentClassID { get; set; }
    public int UserID { get; set; }
}
