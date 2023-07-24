using Application.Tournament.Commands;
using Application.Tournament.Queries;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : BaseController
    {
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateTournamentCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPut("edit")]
        public async Task<IActionResult> Update([FromForm] UpdateTournamentCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetTournaments([FromQuery] GetTournamentQuery filter)
        {
            return ReturnResult(await Mediator.Send(filter));
        }
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> DeleteTournament([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteTournamentCommand { Id = id }));
        }
        [AllowAnonymous]
        [HttpGet("find/{id}")]
        public async Task<ActionResult> GetTournamentWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetTournamentByIdQuery { Id = id }));
        }
    }
}
