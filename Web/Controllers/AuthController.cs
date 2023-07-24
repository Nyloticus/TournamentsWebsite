using Application.Auth.Commands;
using Application.Auth.Queries;
using Common.Attributes;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
          [FromBody] LoginCommand loginDto)
        {
            return ReturnResult((await Mediator.Send(loginDto)));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePassword)
        {
            return ReturnResult((await Mediator.Send(changePassword)));
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand forgetPassword)
        {
            return ReturnResult((await Mediator.Send(forgetPassword)));
        }


        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPassword)
        {
            return ReturnResult((await Mediator.Send(resetPassword)));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }
        [HasPermission(PermissionKeys.AddUser)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

        [HasPermission(RolesKey.SuperAdmin)]
        [HttpGet("get-permissions")]
        public async Task<ActionResult> GetPermissions([FromQuery] GetAllPermissionQuery request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

        [HttpPost("add-permission-to-role")]
        public async Task<ActionResult> AddPermissionToRole([FromBody] AddPermissionToRolCommand request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }
        [HasPermission(PermissionKeys.RemoveUser)]
        [HttpPost("remove-user")]
        public async Task<ActionResult> RemoveUser([FromBody] RemoveUserCommand request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

        [HasPermission(RolesKey.SuperAdmin)]
        [HttpGet("get-active-users")]
        public async Task<ActionResult> GetActiveUsers([FromQuery] GetActiveUsersQuery request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

        [HasPermission(SystemModule.UserManagementModule)]
        [HttpPut("update-user-roles")]
        public async Task<ActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

        [HasPermission(PermissionKeys.ReadRole)]
        [HttpGet("get-app-roles")]
        public async Task<ActionResult> GetAppRoles([FromQuery] GetAppRolesQuery request)
        {
            return ReturnResult((await Mediator.Send(request)));
        }

    }
}