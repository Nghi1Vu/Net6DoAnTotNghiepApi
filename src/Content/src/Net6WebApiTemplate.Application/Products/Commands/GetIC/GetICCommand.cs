using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetICCommand : IRequest<List<IndependentClass>>
{
    public int ModulesID { get; set; }
    public int CourseIndustryID { get; set; }
    public int CourseID { get; set; }
}
