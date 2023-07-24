using Common.Attributes;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Enums.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPermissionService
    {
        List<ModulePermission> GetPermissions();
        List<ModulePermission> GetPermissions(SystemModule modules);
        Task AddPermissionsToRole(Role role, List<PermissionKeys> permission);
        IEnumerable<string> GetModulesPermissions(params SystemModule[] modules);
        Task AddPermissionsToRole(Role role, string[] permission);
        List<string> GetPermissionListForUser(User userId);
        List<string> GetRolePermissions(string RoleId);
        List<GroupedPermission> GetGroupedPermissions();

    }
}
