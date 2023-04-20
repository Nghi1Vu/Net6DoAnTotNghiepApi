using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetStudentInfoCommand : IRequest<StudentInfo>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string email { get; set; }
}
