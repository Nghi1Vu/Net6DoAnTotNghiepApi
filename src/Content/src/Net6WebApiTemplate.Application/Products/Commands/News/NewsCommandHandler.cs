using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class NewsCommandHandler : IRequestHandler<NewsCommand, List<News>>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public NewsCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<List<News>> Handle(NewsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<News> news = _productRepository.GetNews();
            return news;
        }
        catch
        {
            throw new UnauthorizedException("Invalid username or password.");
        }
    }
}
