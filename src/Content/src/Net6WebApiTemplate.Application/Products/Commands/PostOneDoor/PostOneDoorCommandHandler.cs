using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class PostOneDoorCommandHandler : IRequestHandler<PostOneDoorCommand, string>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public PostOneDoorCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<string> Handle(PostOneDoorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string result = _productRepository.PostOneDoor(request.model.UserId, request.model.odid, request.model.amount);
            return result;
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
