using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetModuleDetailCommand : IRequest<List<ModuleDetail>>
{
    public int ModulesID { get; set; }
}
