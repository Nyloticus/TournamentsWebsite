﻿using Common;
//using Domain.Entities.Tenants;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Entities.Auth
{
    public class Role : IdentityRole, IBaseEntity/*, ITenant*/
    {
        public Role(string name) : base(name)
        {
            Name = name;
        }
        public string TenantId { get; set; }
        public string DisplayName { get; set; }
        //public Tenant Tenant { get; set; }

        public HashSet<UserRole> UserRoles { get; set; }
        public string Permissions { get; set; }
    }

}
