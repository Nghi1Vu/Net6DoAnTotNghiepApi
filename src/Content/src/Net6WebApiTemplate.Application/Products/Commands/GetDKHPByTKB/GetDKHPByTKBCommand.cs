﻿using MediatR;
using Net6WebApiTemplate.Application.Products.Dto;
namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
public class GetDKHPByTKBCommand : IRequest<List<DKHPByTKB>>
{
    public int UserID { get; set; }
}
