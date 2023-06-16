﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using Net6WebApiTemplate.Application.Products.Dto;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class PostOneDoorCommandValidator : AbstractValidator<StudentInfo>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public PostOneDoorCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
