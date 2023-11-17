using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Net6WebApiTemplate.Api.Contracts.Version1.Requests;
using Net6WebApiTemplate.Api.Routes.Version1;
using Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
using Net6WebApiTemplate.Application.Products.Commands.DeleteProduct;
using Net6WebApiTemplate.Application.Products.Commands.PatchProduct;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Application.Products.NQueries.GetProductById;
using Net6WebApiTemplate.Application.Products.NQueries.GetProducts;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Net6WebApiTemplate.Api.Controllers.Version1
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;


        public ProductController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;

        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="201">Success creating new product</response>
        /// <response code="400">Bad request</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpPost]
        [Route(ApiRoutes.Product.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            var command = new CreateProductCommand()
            {
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice,
                CategoryId = request.CategoryId
            };

            await _mediator.Send(command);

            return Created(ApiRoutes.Product.Create, command);
        }

        /// <summary>
        /// Retrieve product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        /// <response code="200">Success Retrieve product by Id</response>
        /// <response code="400">Bad request</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpGet]
        [Route(ApiRoutes.Product.Get)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var query = new GetProductByIdQuery()
            {
                Id = id
            };
            await _mediator.Send(query);

            return Ok(query);
        }

        /// <summary>
        ///  Get list of Products
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success retrieving product list</response>
        /// <response code="400">Bad request</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpGet]
        [Route(ApiRoutes.Product.GetAll)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetProductsQuery();
            var results = await _mediator.Send(query);

            return Ok(results);
        }

        /// <summary>
        /// Update an exsiting product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Success updating exsiting product</response>
        /// <response code="400">Bad request</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpPatch]
        [Route(ApiRoutes.Product.Patch)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] ProductRequest request)
        {
            var command = new PatchProductCommand()
            {
                Id = request.Id,
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice,
                CategoryId = request.CategoryId,
            };
            var results = await _mediator.Send(command);

            return Ok(results);
        }

        /// <summary>
        ///  Delete an existing product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Success delete an exsiting product</response>
        /// <response code="400">Bad request</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpDelete]
        [Route(ApiRoutes.Product.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var results = await _mediator.Send(command);

            return Ok(results);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetNews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetNews()
        {
            var query = new NewsCommand()
            {
               
            };
            var rsNews= await _mediator.Send(query);

            return Ok(rsNews);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetStudentClass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetStudentClass(int ClassID)
        {
            var query = new GetStudentClassCommand()
            {
                ClassID= ClassID
            };
            var rsNews = await _mediator.Send(query);

            return Ok(rsNews);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetNewsDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetNewsDetail(int NewsId)
        {
            var query = new NewsDetailCommand()
            {
                NewsId= NewsId
            };
            var rsNews = await _mediator.Send(query);

            return Ok(rsNews);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/GetStudentInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentInfo(GetStudentInfoCommand query)
        {
            try
            {
                var rsInfo = await _mediator.Send(query);
                if (rsInfo != null && rsInfo.UserId > 0)
                {
                    var claims = new[]
                   {
    new Claim(JwtRegisteredClaimNames.Sub, rsInfo.Usercode),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};
                    var stkey = _configuration["SecretKey"].ToString();
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(stkey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: _configuration["issuer"],
                    audience: _configuration["audience"],
                    claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(5),
                        signingCredentials: creds);

                    return Ok(new { rsInfo, Key = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return Ok(rsInfo);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/GetStudentInfoByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStudentInfoByEmail(GetStudentInfoByEmailCommand query)
        {
            var rsInfo = await _mediator.Send(query);
            if (rsInfo != null && rsInfo.UserId>0)
            {
                var claims = new[]
               {
    new Claim(JwtRegisteredClaimNames.Sub, rsInfo.Usercode),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};
                var stkey = _configuration["SecretKey"].ToString();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(stkey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["issuer"],
                audience: _configuration["audience"],
                claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    signingCredentials: creds);

                return Ok(new { rsInfo, Key = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else
            {
                return Ok(rsInfo);
            }
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetStudentDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetStudentDetail(int UserId)
        {
            var query = new GetStudentDetailCommand()
            {
                UserId = UserId,
            };
            var rsInfo = await _mediator.Send(query);

            return Ok(rsInfo);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetFamilyDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetFamilyDetail(int UserId)
        {
            var query = new GetFamilyDetailCommand()
            {
                UserId = UserId,
            };
            var rsInfo = await _mediator.Send(query);

            return Ok(rsInfo);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetRLSemester")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetRLSemester(int UserId)
        {
            var query = new GetRLSemesterCommand()
            {
                UserId = UserId,
            };
            var rsInfo = await _mediator.Send(query);

            return Ok(rsInfo);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetRLForm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetRLForm()
        {
            var query = new GetRLFormCommand()
            {
            
            };
            var rsInfo = await _mediator.Send(query);

            return Ok(rsInfo);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/PostRLForm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> PostRLForm(PostRLForm model)
        {
            var query = new PostRLFormCommand()
            {
                model = model
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/PostTTCN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> PostTTCN(PostTTCN model)
        {
            var query = new PostTTCNCommand()
            {
                model = model
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/PostOneDoor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> PostOneDoor(PostOneDoor model)
        {
            var query = new PostOneDoorCommand()
            {
                model = model
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetProgramSemester")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetProgramSemester(int CourseIndustryID, int CourseID, int UserID)
        {
            var query = new GetProgramSemesterCommand()
            {
                CourseIndustryID= CourseIndustryID,
                CourseID= CourseID,
                UserID= UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetModuleDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetModuleDetail(int ModulesID)
        {
            var query = new GetModuleDetailCommand()
            {
                ModulesID= ModulesID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetIC")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetIC(int ModulesID, int CourseID, int CourseIndustryID)
        {
            var query = new GetICCommand()
            {
                ModulesID = ModulesID,
                CourseIndustryID= CourseIndustryID,
                CourseID= CourseID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetKQHTByUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetKQHTByUser(int UserID)
        {
            var query = new GetKQHTByUserCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetKQHTByClass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetKQHTByClass(int IndependentClassID)
        {
            var query = new GetKQHTByClassCommand()
            {
                IndependentClassID = IndependentClassID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetCertificateByUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetCertificateByUser(int UserID, int CourseIndustryID)
        {
            var query = new GetCertificateByUserCommand()
            {
                UserID = UserID,
                CourseIndustryID= CourseIndustryID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetDsGtHs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetDsGtHs(int UserID)
        {
            var query = new GetDsGtHsCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTradeHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTradeHistory(int UserID)
        {
            var query = new GetTradeHistoryCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetMessage(int ClassID)
        {
            var query = new GetMessageCommand()
            {
                ClassID = ClassID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/PostMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> PostMessage(int UserID, string content)
        {
            var query = new PostMessageCommand()
            {
                UserID = UserID,
                content= content
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTTCN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTTCN(int UserID)
        {
            var query = new GetTTCNCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTTCNDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTTCNDone(int UserID)
        {
            var query = new GetTTCNDoneCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetStudentAmount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetStudentAmount(int UserID)
        {
            var query = new GetStudentAmountCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetChannelAmount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetChannelAmount(int ClassID)
        {
            var query = new GetChannelAmountCommand()
            {
                ClassID = ClassID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetExamResult")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetExamResult(int UserID)
        {
            var query = new GetExamResultCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetExamByClass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetExamByClass(int IndependentClassID)
        {
            var query = new GetExamByClassCommand()
            {
                IndependentClassID = IndependentClassID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetExamCalendar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetExamCalendar(int UserID)
        {
            var query = new GetExamCalendarCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTeachCalendar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTeachCalendar(int UserID)
        {
            var query = new GetTeachCalendarCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTeachCalendarDetail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTeachCalendarDetail(int IndependentClassID, int UserID)
        {
            var query = new GetTeachCalendarDetailCommand()
            {
                IndependentClassID= IndependentClassID,
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTBCHK")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTBCHK( int UserID)
        {
            var query = new GetTBCHKCommand()
            {
                UserID = UserID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetTKB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetTKB(int UserID, string? aDate, string? eDate)
        {
            var query = new GetTKBCommand()
            {
                UserID = UserID,
                aDate= aDate,
                eDate= eDate
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetDKHPByTKB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetDKHPByTKB(int UserID)
        {
            var query = new GetDKHPByTKBCommand()
            {
                UserID = UserID,
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/DeleteDKHP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> DeleteDKHP(DeleteDKHPCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetLogDKHP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetLogDKHP(int UserID)
        {
            var query = new GetLogDKHPCommand()
            {
                UserID = UserID,
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/v{version:apiVersion}/GetICByTKB")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> GetICByTKB(int TimesInDay, int DayStudy, int CourseIndustryID, int CourseID)
        {
            var query = new GetICByTKBCommand()
            {
                TimesInDay = TimesInDay,
                DayStudy = DayStudy,
                CourseIndustryID= CourseIndustryID,
                CourseID= CourseID
            };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost]
        [Route("/api/v{version:apiVersion}/HandleDKHP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = "JwtBaerer")]
        public async Task<IActionResult> HandleDKHP(HandleDKHPCommand handle)
        {
            var result = await _mediator.Send(handle);

            return Ok(result);
        }
    }
}