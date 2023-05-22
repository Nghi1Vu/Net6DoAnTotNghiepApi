using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetExamCalendarCommand : IRequest<List<ExamCalendar>>
{
    public int UserID { get; set; }
}
