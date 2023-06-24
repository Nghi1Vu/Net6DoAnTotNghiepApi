using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class DeleteDKHPCommandHandler : IRequestHandler<DeleteDKHPCommand, string>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public DeleteDKHPCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<string> Handle(DeleteDKHPCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string result = _productRepository.DeleteDKHP(request.UserID, request.IndependentClassID);
            return result;
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
