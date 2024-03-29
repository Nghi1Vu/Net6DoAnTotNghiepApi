﻿using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetRLFormCommand : IRequest<List<RLForm>>
{
    public int UserId { get; set; }
}
