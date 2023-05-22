using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetExamResultCommand : IRequest<List<ExamResult>>
{
    public int UserID { get; set; }
}
