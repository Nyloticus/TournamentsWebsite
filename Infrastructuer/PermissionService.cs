using Common.Attributes;
using Common.Extensions;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Enums.Roles;
using Domain.Extensions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PermissionService : IPermissionService
    {
        private readonly IServiceProvider _scope;
        private readonly IAppDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public PermissionService(IServiceProvider scope, IAppDbContext context, RoleManager<Role> roleManager)
        {
            _scope = scope;
            _context = context;
            _roleManager = roleManager;
        }
        private ModulePermission GetModulePermission(SystemModule module)
        {
            var list = GetModulePermissions(module)
                 .GroupBy(p => p.GetAttribute<DescribePermissionAttribute>().Group)
                 .Select(s => new GroupedPermission()
                 {
                     Name = s.Key,
                     Permissions = s.Select(p => p.GetAttribute<DescribePermissionAttribute>().Title).ToList()
                 }
                 ).ToList();

            return new ModulePermission()
            {
                Name = module.ToString(),
                Permissions = list
            };
        }

        public List<ModulePermission> GetPermissions()
        {
            var modulePermissions = new List<ModulePermission>();
            foreach (SystemModule module in Enum.GetValues(typeof(SystemModule)))
            {
                modulePermissions.Add(GetModulePermission(module));
            }
            return modulePermissions;
        }

        public List<ModulePermission> GetPermissions(SystemModule modules)
        {
            var modulePermissions = new List<ModulePermission>();


            foreach (SystemModule module in Enum.GetValues(typeof(SystemModule)))
            {
                if (modules.HasFlag(module))
                {
                    modulePermissions.Add(GetModulePermission(module));
                }
            }
            return modulePermissions;
        }

        public List<GroupedPermission> GetGroupedPermissions()
        {
            var groupedPermissions = new List<GroupedPermission>();

            foreach (SystemModule module in Enum.GetValues(typeof(SystemModule)))
            {

                var list = GetModulePermissions(module)
                  .GroupBy(p => p.GetAttribute<DescribePermissionAttribute>().Group)
                  .Select(s => new GroupedPermission()
                  {
                      Name = s.Key,
                      Permissions = s.Select(p => p.GetAttribute<DescribePermissionAttribute>().Title).ToList()
                  }
                  ).ToList();

                groupedPermissions.AddRange(list);

            }
            return groupedPermissions;
        }

        private IEnumerable<PermissionKeys> GetModulePermissions(SystemModule module)
        {
            foreach (PermissionKeys item in Enum.GetValues(typeof(PermissionKeys)))
            {
                if (item.GetAttribute<DescribePermissionAttribute>().Module == module)
                    yield return item;
            }
        }

        //public IEnumerable<PermissionKeys> GetModulesPermissions(SystemModule modules)
        //{
        //    foreach (SystemModule module in Enum.GetValues(typeof(SystemModule)))
        //    {
        //        if (modules.HasFlag(module))
        //        {
        //            foreach (PermissionKeys item in Enum.GetValues(typeof(PermissionKeys)))
        //            {
        //                if (item.GetAttribute<DescribePermissionAttribute>().Module == module)
        //                    yield return item;
        //            }
        //        }
        //    }
        //}
        public IEnumerable<string> GetModulesPermissions(params SystemModule[] modules)
        {
            var permissionsTitle = new List<string>();
            foreach (var module in modules)
            {
                var modulePermissions = typeof(PermissionKeys).GetFields()
                         .Where(x => x.GetCustomAttribute<DescribePermissionAttribute>()?.Module == module)
                         .Select(x => x.GetValue(null)).Cast<PermissionKeys>().getPermissionTitle();

                permissionsTitle.AddRange(modulePermissions);
            }
            return permissionsTitle;
        }

        public async Task AddPermissionsToRole(Role role, List<PermissionKeys> permissions)
        {
            foreach (var permission in permissions)
            {
                var userManager = _scope.GetService<UserManager<User>>();


                role.Permissions ??= "";
                var moduleAttribute = permission.GetAttribute<DescribePermissionAttribute>();
                if (!role.Permissions.Contains((char)permission))
                {
                    role.Permissions += (char)permission;
                    if (moduleAttribute != null)
                    {
                        SystemModule module = moduleAttribute.Module;
                        var users = await userManager.GetUsersInRoleAsync(role.Name);
                        foreach (var user in users)
                        {
                            if (((user.AllowedModules & module) == 0))
                            {
                                //add user to this module
                                user.AllowedModules |= module;
                            }

                            await userManager.UpdateAsync(user);
                        }
                    }
                }
            }
        }

        public async Task AddPermissionsToRole(Role role, string[] permissions)
        {
            await AddPermissionsToRole(role, ConvertStingToPermisson(permissions));
        }

        private List<PermissionKeys> ConvertStingToPermisson(string[] strs)
        {
            var attrs = (typeof(PermissionKeys)).GetFields().ToList();

            List<PermissionKeys> permissionsList = new List<PermissionKeys>();
            foreach (var str in strs)
            {
                foreach (var attr in attrs)
                {
                    if (attr.GetCustomAttribute<DescribePermissionAttribute>() != null)
                    {
                        if (str == attr.GetCustomAttribute<DescribePermissionAttribute>().Title)
                            permissionsList.Add((PermissionKeys)attr.GetValue((typeof(PermissionKeys))));
                    }
                }
            }


            return permissionsList;
        }

        public List<string> GetPermissionListForUser(User appUser)
        {
            var permissionsList = appUser.UserRoles
            .Select(ur => ur.Role.Permissions.GetPermissionsFromString().getPermissionTitle());


            List<string> permissions = new List<string>();

            foreach (var permission in permissionsList)
            {
                permission.ToList().ForEach(p =>
                {
                    permissions.Add(p);
                });
            }


            return permissions;
        }
        public List<string> GetRolePermissions(string RoleId)
        {
            List<string> permissionsTitle = new List<string>();

            var permissionList = _roleManager.FindByIdAsync(RoleId).Result.Permissions.GetPermissionsFromString().getPermissionTitle();

            foreach (var permission in permissionList)
            {
                permissionsTitle.Add(permission);
            }
            return permissionsTitle;
        }
    }


}