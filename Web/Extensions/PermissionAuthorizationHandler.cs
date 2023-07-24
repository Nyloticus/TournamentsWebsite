using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissions = context.User.Claims.FirstOrDefault(x => x.Type == "permissions");
            if (permissions != null)
            {
                var permissionsSerlizaed = context.User.Claims.FirstOrDefault(x => x.Type == "permissions").Value;
                var permissionsDeserlizaed = JsonConvert.DeserializeObject<List<string>>(permissionsSerlizaed);


                var requiredPermissionLs = requirement.Permission.Split(",").ToList();
                bool IsAuthorized = false;

                foreach (var permission in requiredPermissionLs)
                {
                    if (permissionsDeserlizaed.Contains(permission)) IsAuthorized = true;
                }
                if (!IsAuthorized) context.Fail();
                context.Succeed(requirement);
            }
        }
    }
}
