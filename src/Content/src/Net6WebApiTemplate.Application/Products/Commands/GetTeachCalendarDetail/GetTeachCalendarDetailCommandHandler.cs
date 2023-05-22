using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;

public class GetTeachCalendarDetailCommandHandler : IRequestHandler<GetTeachCalendarDetailCommand, List<TeachCalendarDetail>>
{
    private readonly IMediator _mediator;
    private readonly INet6WebApiTemplateDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    public GetTeachCalendarDetailCommandHandler(IMediator mediator, INet6WebApiTemplateDbContext dbContext, IProductRepository productRepository)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _productRepository = productRepository;
    }

    public async Task<List<TeachCalendarDetail>> Handle(GetTeachCalendarDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<TeachCalendarDetail> result = _productRepository.GetTeachCalendarDetail(request.IndependentClassID, request.UserID);
            return result;
        }
        catch
        {
            throw new UnauthorizedException("Invalid.");
        }
    }
}
