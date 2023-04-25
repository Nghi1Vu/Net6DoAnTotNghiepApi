using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetStudentInfoByEmailCommand : IRequest<StudentInfo>
{
    public string email { get; set; }
}
