using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetRLSemesterCommand : IRequest<List<RLSemester>>
{
    public int UserId { get; set; }
}
