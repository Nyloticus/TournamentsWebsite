using Application.TournamentTeam.Commands;
using Application.TournamentTeamTeam.Queries;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentTeamController : BaseController
    {
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTeamToTournament([FromBody] AssignTeamToTournamentCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HasPermission(RolesKey.SuperAdmin)]
        [HttpPost("remove-assign")]
        public async Task<IActionResult> RemoveTeamFromTournament([FromBody] RemoveTeamFromTournamentCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HasPermission(RolesKey.SuperAdmin)]
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] GetTournamentTeamQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
    }
}
