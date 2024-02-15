using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.InfraStructure.Cors;
using System.Reflection.Metadata;
using System.Text;

namespace MovieBooking.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        //Accept the incoming request
       //· Handles the incoming request and provide the response.
        private ISender _mediator = null!;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected string GetOriginFromRequest(IConfiguration _configuration)
        {
            string requestSource = Convert.ToString(HttpContext.Request.Headers["request-source"]);

            var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
           
            return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        }
    }
}
