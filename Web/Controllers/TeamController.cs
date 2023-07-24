using Application.Team.Commands;
using Application.Team.Queries;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : BaseController
    {
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateTeamCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPut("edit")]
        public async Task<IActionResult> Update([FromForm] UpdateTeamCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetTeams([FromQuery] GetTeamQuery filter)
        {
            return ReturnResult(await Mediator.Send(filter));
        }

        [HasPermission(RolesKey.SuperAdmin)]
        [HttpGet("unassigned")]
        public async Task<IActionResult> GetUnassignedTeams([FromQuery] GetUnassignedTeamsQuery filter)
        {
            return ReturnResult(await Mediator.Send(filter));
        }
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> DeleteTeam([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteTeamCommand { Id = id }));
        }

        [HttpGet("find/{id}")]
        public async Task<ActionResult> GetTeamWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetTeamByIdQuery { Id = id }));
        }
    }
}
