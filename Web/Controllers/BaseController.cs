using Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Web.Controllers
{
    [Authorization]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        protected ObjectResult ReturnResult(Result result)
        {
            switch (result.Code)
            {
                case ApiExeptionType.Ok:
                    return Ok(result);
                // case HttpStatusCode.Created:
                //   return Created("", result);
                case ApiExeptionType.BadRequest:
                    return BadRequest(result);
                case ApiExeptionType.Unauthorized:
                    return Unauthorized(result);
                case ApiExeptionType.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }
    }
}