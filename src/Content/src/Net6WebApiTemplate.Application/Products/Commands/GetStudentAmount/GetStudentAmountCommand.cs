using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetStudentAmountCommand : IRequest<List<StudentAmount>>
{
    public int UserID { get; set; }
}
