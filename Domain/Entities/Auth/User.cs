using Common;
using Common.Attributes;
using Common.Interfaces;
//using Domain.Entities.Tenants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Auth
{
    //[Table("AspNetUsers")]
    public class User : IdentityUser, ICreateAudit, IUpdateAudit, IDeleteEntity, IBaseEntity/*,ITenant */
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public SystemModule AllowedModules { get; set; }

        public string Permissions { get; set; }
        public bool Active { get; set; } = true;
        public string Avatar { get; set; }

        public string TenantId { get; set; }


        public string StoreId { get; set; }
        public string CityId { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool? IsSeen { get; set; }



        public string NationalId { get; set; }
        public string NationalImg { get; set; }
        public int Otp { get; set; }


        public HashSet<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public HashSet<UserImages> UserImages { get; set; } = new HashSet<UserImages>();
        public HashSet<UserDevice> UserDevices { get; set; } = new HashSet<UserDevice>();
        public SystemModule Modules { get; set; }

        //public Tenant Tenant { get; set; }
    }

}