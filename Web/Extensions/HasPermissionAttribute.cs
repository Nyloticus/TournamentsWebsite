using Common.Attributes;
using Domain.Enums.Roles;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Web.Extensions
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(params PermissionKeys[] permissions)
        {
            var permissionsTitle = permissions.Select(x => x.GetAttributeOfType<DescribePermissionAttribute>().Title);
            Policy = string.Join(",", permissionsTitle);
        }
        public HasPermissionAttribute(params SystemModule[] modules)
        {
            var permissionsTitle = new List<string>();
            foreach (var module in modules)
            {
                var modulePermissions = typeof(PermissionKeys).GetFields()
                         .Where(x => x.GetCustomAttribute<DescribePermissionAttribute>()?.Module == module)
                         .Select(x => x.GetValue(null)).Cast<PermissionKeys>().getPermissionTitle();

                permissionsTitle.AddRange(modulePermissions);
            }
            Policy = string.Join(",", permissionsTitle);
        }
        public HasPermissionAttribute(params RolesKey[] roles)
        {
            Roles = string.Join(",", roles.Select(x => (x.ToString())));
        }
    }
}
