﻿using Common.Attributes;
using Common.Extensions;
using Domain.Entities.Auth;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public partial class AppDbInitializer
    {
        public async Task SeedAuthEverything(AppDbContext context, IServiceScope serviceScope, string contentPath)
        {
            var created = await context.Database.EnsureCreatedAsync();

            if (!context.Users.Any())
            {
                SeedUsers(context);
            }

            if (!context.Roles.Any())
            {
                SeedRoles(context);
            }

            //else { SeedRoles(context); }

            if (!context.UserRoles.Any())
            {
                SeedUserRoles(context);
            }

            await SeedSuperAdminPermission(context);
            await SeedPredefineUsers(serviceScope, contentPath);
            //await SeedAppRolesPermssions(context);
        }

        private async Task SeedPredefineUsers(IServiceScope serviceScope, string root)
        {
            try
            {
                using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>())
                {
                    var file = await File.ReadAllTextAsync(Path.Combine(root, "seeds", "predefineusers.json"));
                    var users = JsonConvert.DeserializeObject<List<UserJson>>(file);
                    foreach (var u in users)
                    {
                        var user = await userManager.FindByNameAsync(u.Username);
                        if (user != null)
                            continue;

                        user = new User()
                        {
                            UserName = u.Username.ToLower(),
                            Email = u.Email.ToLower(),
                            NormalizedEmail = u.Email.ToUpper(),
                            NormalizedUserName = u.Username.ToUpper()
                        };
                        var result = await userManager.CreateAsync(user, u.Password);
                        await userManager.AddToRoleAsync(user, u.RoleName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task SeedSuperAdminPermission(AppDbContext context)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == RolesKey.SuperAdmin.ToString());

            var permissions = Enum.GetValues(typeof(PermissionKeys));
            role.Permissions = "";
            foreach (PermissionKeys permission in permissions)
            {
                var moduleAttribute = permission.GetAttribute<DescribePermissionAttribute>();
                role.Permissions += (char)permission;
            }

            await context.SaveChangesAsync();
        }
        //private async Task SeedAppRolesPermssions(AppDbContext context)
        //{
        //    var roles = await context.Roles.ToListAsync();
        //    var permissions = Enum.GetValues(typeof(PermissionKeys));

        //    foreach (var role in roles)
        //    {
        //        if (role.Name == RolesKey.Maindata.ToString())
        //        {
        //            var mainDataPermissions = permissions.OfType<PermissionKeys>().Where(p => p.GetAttribute<DescribePermissionAttribute>().Group == "maindata");
        //            role.Permissions = "";
        //            foreach (PermissionKeys permission in mainDataPermissions)
        //            {
        //                role.Permissions += (char)permission;
        //            }
        //        }
        //    }
        //    await context.SaveChangesAsync();
        //}

        public void SeedUsers(AppDbContext context)
        {
            var users = new[] {
        new User() {
          AccessFailedCount = 0,
          ConcurrencyStamp = Guid.NewGuid().ToString(),
          Email = "Admin@Apps.com",
          EmailConfirmed = true,
          Id = Guid.NewGuid().ToString(),
          LockoutEnabled = false,
          LockoutEnd = null,
          NormalizedEmail = ("Admin@Apps.com").ToUpper(),
          NormalizedUserName = ("Admin").ToUpper(),
          PasswordHash = "AQAAAAEAACcQAAAAEIsWjNQ5zjWAxoVt9Hr9Z3XUpWtkXhhil17iNtANiIuQnkIGRynUkDy529Cqpk/Epg==",
          PhoneNumber = "",
          PhoneNumberConfirmed = true,
          SecurityStamp = Guid.NewGuid().ToString(),
          TwoFactorEnabled = false,
          UserName = "Admin",
          CreatedDate = DateTime.UtcNow,
          CreatedBy = "System"
        },
      };
            context.Users.AddRange(users);

            context.SaveChanges();
        }

        public void SeedRoles(AppDbContext context)
        {
            var roles = new List<Role>();
            var dbRoles = context.Roles.ToList();
            foreach (RolesKey item in Enum.GetValues(typeof(RolesKey)))
            {
                if (!dbRoles.Any(a => a.Name == item.ToString()))
                    roles.Add(new Role(item.ToString()) { NormalizedName = item.ToString().ToUpper() });
            }

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        public void SeedUserRoles(AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == "Admin");
            var role = context.Roles.FirstOrDefault(r => r.Name == RolesKey.SuperAdmin.ToString());

            context.UserRoles.Add(new UserRole() { RoleId = role?.Id, UserId = user?.Id });
            context.SaveChanges();
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
              .Where(x => x % 2 == 0)
              .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
              .ToArray();
        }
    }

    public class UserJson
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}