using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetProgramSemesterCommand : IRequest<List<ProgramSemester>>
{
    public int CourseIndustryID { get; set; }
    public int CourseID { get; set; }
    public int UserID { get; set; }
}
