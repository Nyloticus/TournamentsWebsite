using Common.Interfaces;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class AppUserManager : UserManager<User>
    {
        private readonly IAuditService _auditService;
        private static Random random = new Random();
        public AppUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger, IAuditService auditService) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _auditService = auditService;
        }
        public override Task<IdentityResult> CreateAsync(User user)
        {
            if (user is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(User user, string password)
        {
            if (user is ITenant tenant)
                tenant.TenantId = _auditService.TenantId;

            return base.CreateAsync(user, password);
        }


    }
}