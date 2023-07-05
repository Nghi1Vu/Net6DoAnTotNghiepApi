﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net6WebApiTemplate.Api.Contracts.Version1.Requests;
using Net6WebApiTemplate.Api.Routes.Version1;
using Net6WebApiTemplate.Application.Auth.Commands.SignIn;

namespace Net6WebApiTemplate.Api.Controllers.Version1
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///  Authenticate user's credentials
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">User authentication successful</response>
        /// <response code="401">Unauthorized</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpPost]
        [Route(ApiRoutes.Auth.SignIn)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var command = new SignInCommand
            {
                Username = request.Username.ToLower().Trim(),
                Password = request.Password.Trim()
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost]
        [Route(ApiRoutes.Auth.LoginWithEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginWithEmail(LoginWithEmailRequest request)
        {
            var command = new LoginWithEmailCommand
            {
                email = request.email.ToLower().Trim()
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand change)
        {
            var response = await _mediator.Send(change);
            return Ok(response);
        }
        [HttpGet]
        [Route(ApiRoutes.Auth.GetIndex)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIndex()
        {
            return Content("Hello");
        }
    }
}