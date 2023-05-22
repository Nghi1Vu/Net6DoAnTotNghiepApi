using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetTeachCalendarCommand : IRequest<List<TeachCalendar>>
{
    public int UserID { get; set; }
}
