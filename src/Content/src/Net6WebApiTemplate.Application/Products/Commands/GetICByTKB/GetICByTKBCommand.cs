using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetICByTKBCommand : IRequest<List<IndependentClass>>
{
    public int DayStudy { get; set; }
    public int TimesInDay { get; set; }
    public int CourseIndustryID { get; set; }
    public int CourseID { get; set; }
}
