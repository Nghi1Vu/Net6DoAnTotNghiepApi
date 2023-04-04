﻿using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        //Task<Product?> GetById(long id);
        bool SignIn(string Username, string Password);
    }
}