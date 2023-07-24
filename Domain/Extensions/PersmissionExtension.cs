using Common.Attributes;
using Common.Extensions;
using Domain.Enums.Roles;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Extensions
{
    public static class PersmissionExtension
    {
        public static IEnumerable<string> getPermissionTitle(this IEnumerable<PermissionKeys> permissions)
        {
            return permissions.Select(s => s.GetAttribute<DescribePermissionAttribute>().Title).ToArray();
        }
        public static IEnumerable<PermissionKeys> GetPermissionsFromString(this string permissions)
        {
            if (!string.IsNullOrEmpty(permissions))
            {
                foreach (var chr in permissions)
                {
                    yield return (PermissionKeys)chr;
                }
            }

        }
    }
}