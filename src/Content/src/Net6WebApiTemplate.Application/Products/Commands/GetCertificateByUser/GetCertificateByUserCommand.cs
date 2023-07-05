using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetCertificateByUserCommand : IRequest<List<Certificate>>
{
    public int UserID { get; set; }
    public int CourseIndustryID { get; set; }
}
