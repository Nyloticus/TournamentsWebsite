using Common;
using Common.Extensions;
using Common.Options;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuditService _auditService;
        private readonly Common.Interfaces.IUrlHelper _urlHelper;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPermissionService _permissionServices;
        private readonly JwtOption _jwtOption;
        private readonly IAppDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(UserManager<User> userManager,
          IAuditService auditService,
          Common.Interfaces.IUrlHelper urlHelper,
          RoleManager<Role> roleManager,
          IPermissionService permissionServices,
          JwtOption jwtOption,
          IAppDbContext context,
          TokenValidationParameters tokenValidationParameters
        )
        {
            _userManager = userManager;
            _auditService = auditService;
            _roleManager = roleManager;
            _jwtOption = jwtOption;
            _context = context;
            _urlHelper = urlHelper;
            _permissionServices = permissionServices;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<Result> ValidateUserPassword(string userId, string password)
        {
            var user = await _userManager.Users.Protected()
             .FirstOrDefaultAsync(e => e.Id == userId);

            if (user == null)
            {
                return Result.Successed(false);
            }
            if (!user.Active)
                return Result.Successed(false);

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
                return Result.Successed(false);

            return Result.Successed(true);

        }

        public async Task<Result> LoginAsync(string email, string password, string deviceToken)
        {
            //var user = await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .Protected()
              .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() || u.UserName.ToLower() == email.ToLower());


            if (user == null)
            {
                return Result.Failure(ApiExeptionType.NotFound, "Invalid Login");
            }

            if (!user.Active)
                return Result.Failure(ApiExeptionType.BadRequest, "This user has been disabled");



            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
                throw new ApiException(ApiExeptionType.InvalidLogin);

            // if (user.UserRoles.Select(s => s.Role.Name).Contains(RolesKey.Promoter.ToString())) {
            //   var promoter = await _context.Promoters
            //     .Include(u => u.UserRoles)
            //     .ThenInclude(u => u.Role)
            //     .FirstOrDefaultAsync(p => p.Id == user.Id);
            //   return await GenerateAuthenticationResultForUserAsync(promoter);
            // }
            await AssignDeviceTokenAsync(user, deviceToken);
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task AssignDeviceTokenAsync(User user, string deviceToken)
        {
            if (!string.IsNullOrEmpty(deviceToken))
            {
                var userDevice = await _context.UserDevices.Protected(a => a.DeviceToken == deviceToken).FirstOrDefaultAsync();
                if (userDevice != null)
                {
                    userDevice.UserId = user.Id;
                    _context.UserDevices.Update(userDevice);
                }
                else
                {
                    userDevice = new UserDevice();
                    userDevice.UserId = user.Id;
                    userDevice.DeviceToken = deviceToken;
                    await _context.UserDevices.AddAsync(userDevice);
                }
            }
        }

        public async Task<Result> LogoutAsync(string devicetoken)
        {
            if (!string.IsNullOrEmpty(devicetoken))
            {
                var userDevice = await _context.UserDevices.FirstOrDefaultAsync(e => e.DeviceToken == devicetoken);
                if (userDevice != null)
                {
                    try
                    {
                        _context.UserDevices.Remove(userDevice);
                    }
                    catch (Exception)
                    {
                        throw new ApiException(ApiExeptionType.DeleteRelatedObjectError);
                    }

                    return Result.Successed();
                }
                else
                {
                    throw new ApiException(ApiExeptionType.NotFound);
                }
            }
            else
            {
                throw new ApiException(ApiExeptionType.NotFound);
            }
        }

        public async Task<Result> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            var storedRefreshToken = await _context.RefreshTokens
              .SingleOrDefaultAsync(e => e.Token == refreshToken);

            if (validatedToken == null)
            {
                storedRefreshToken.Invalidated = true;
                return Result.Failure(ApiExeptionType.Unauthorized, "Invalid Token");
            }

            if (storedRefreshToken == null)
            {
                return Result.Failure(ApiExeptionType.Unauthorized, "Refresh token not exist");
            }

            if (storedRefreshToken.Invalidated)
            {
                Result.Failure(ApiExeptionType.Unauthorized, "Invalid refresh token");
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                storedRefreshToken.Invalidated = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                Result.Failure(ApiExeptionType.Unauthorized, "Refresh token expired");
            }
            if (storedRefreshToken.Used)
            {
                // error = new ApiException(ApiExeptionType.Unauthorized, "Refresh token used");
            }

            var userId = validatedToken.Claims.Single(e => e.Type == "UserId").Value;
            var user = await _userManager.Users
              .Include(u => u.UserRoles)
              .ThenInclude(u => u.Role)
              .Protected(u => u.Active)
              .FirstOrDefaultAsync(u => u.Id == userId);


            if (user == null)
            {
                Result.Failure(ApiExeptionType.Unauthorized, "User not found");
            }

            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();


            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var principals = tokenHandler.ValidateToken(token, _tokenValidationParameters,
                  out var validatedToken);
                _tokenValidationParameters.ValidateLifetime = true;
                return !ISJwtWithValidSecurityAlgorithm(validatedToken) ? null : principals;
            }
            catch
            {
                return null;
            }
        }
        private bool ISJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                     StringComparison.InvariantCultureIgnoreCase);
        }






        public async Task<Result> GenerateAuthenticationResultForUserAsync(User user)
        {

            return Result.Successed(new
            {
                Token = await GenerateJwtToken(user.Email, user),
                RefreshToken = (await GenerateRefreshTokenAsync(user)).Token,
                UserName = user.UserName
                //Token_Expire_Date = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            });
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.Set<RefreshToken>().AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<string> GenerateJwtToken(string email, User appUser)
        {
            var permissions = _permissionServices.GetPermissionListForUser(appUser);
            var claims = GenerateUserClaims(appUser, permissions);
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            //claims.AddRange((permissions);
            claims.AddRange(userClaims);
            var userRoles = await _userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(_jwtOption.TokenLifetime);

            var token = new JwtSecurityToken(
              _jwtOption.Issuer,
              _jwtOption.Issuer,
              claims,
              expires: expires,
              signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> GenerateUserClaims(User appUser, List<string> permissions)
        {
            return new List<Claim> {
        new Claim(ClaimTypes.Name, appUser.UserName),
        //new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, appUser.Id),
        new Claim("UserId", appUser.Id),
        new Claim("allowedModules", appUser.AllowedModules.ToString()),
        new Claim("permissions", JsonConvert.SerializeObject(permissions)),
        new Claim("email", appUser.Email??""),
        new Claim("fullName", appUser.FullName??""),
        new Claim("role", JsonConvert.SerializeObject(appUser.UserRoles.Select(s => s.Role.Name))),
        new Claim("isActive", appUser.Active.ToString()),
        new Claim("isPhoneNumberConfirmed", appUser.PhoneNumberConfirmed.ToString()),
        new Claim("PhoneNumber", appUser.PhoneNumber??string.Empty),
        //new Claim("isApproved", (appUser.Status !=null ? appUser.Status.ToString() : string.Empty) ?? string.Empty),
      };
        }

        public async Task<Result> ChangePasswordAsync(string password, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(_auditService.UserId);
            if (user == null)
            {
                return Result.Failure(ApiExeptionType.BadRequest, "User does not exist");
            }

            var result = await _userManager.ChangePasswordAsync(user, password, newPassword);
            return !result.Succeeded ? Result.Failure(ApiExeptionType.NotFound, "", result.Errors.Select(s => new ErrorResult(s.Code, s.Description))) : Result.Successed(null, "password changed successfully");
        }

        public async Task<Result> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(ApiExeptionType.NotFound, "User does not exist");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $" auth/reset-password?token={token}&email=+{user.Email}";

            var callback = _urlHelper.GetCurrentUrl(url);
            return Result.Successed(callback);
        }

        public async Task<Result> ResetPasswordAsync(string token, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(ApiExeptionType.NotFound, "User does not exist");
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
            return resetPassResult.Succeeded ? Result.Successed() : Result.Failure(ApiExeptionType.BadRequest, string.Empty, resetPassResult.Errors.Select(s => new ErrorResult(s.Code, s.Description)));
        }
        public async Task<Result> RegisterAsync(string firstName, string lastName, string email, string phoneNumber, string username, string password, List<string> RolesIds)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            if (user != null)
                return Result.Failure(ApiExeptionType.ValidationError, "User already exists");
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                Email = username,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                Active = true,
            };
            var Password = _userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = Password;
            var newUser = await _userManager.CreateAsync(user);


            var rolesString = await _roleManager.Roles.Where(r => RolesIds.Contains(r.Id)).Select(r => r.Name).ToListAsync();
            await _userManager.AddToRolesAsync(user, rolesString);
            return Result.Successed("User created successfully");
        }
        public async Task<Result> RemoveUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(ApiExeptionType.NotFound, "User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => r.ToLower().Contains("admin") || r.ToLower().Contains("superadmin")))
                return Result.Failure(ApiExeptionType.ValidationError, "You can't remove an admin or superadmin user");

            var result = await _userManager.DeleteAsync(user);
            user.Active = false;
            return result.Succeeded ? Result.Successed("User deleted successfully") : Result.Failure(ApiExeptionType.NotFound, "User not found");
        }
        public async Task<Result> GetActiveUsers()
        {
            var users = _userManager.Users.Where(s => s.Active == true || s.IsDeleted == false).ToList();

            //remove admins and super admins from the list
            var superAdminUsers = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            var updatedUsers = users.Except(superAdminUsers).Except(adminUsers);

            var userData = updatedUsers.Select(s => new
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                UserName = s.UserName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Roles = _roleManager.Roles.Where(role => _userManager.GetRolesAsync(s).Result.Contains(role.Name)).Select(role => new { Id = role.Id, Name = role.Name }).ToList(),
            });

            return Result.Successed(userData);
        }
        public async Task<Result> UpdateUserRoles(string userId, List<string> RolesIds)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(ApiExeptionType.NotFound, "User not found");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => r.ToLower().Contains("admin") || r.ToLower().Contains("superadmin")))
                return Result.Failure(ApiExeptionType.ValidationError, "You can't update an admin or superadmin user");

            var userRolesDeleteResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            var rolesString = await _roleManager.Roles.Where(r => RolesIds.Contains(r.Id)).Select(r => r.Name).ToListAsync();
            var userRolesAddResult = await _userManager.AddToRolesAsync(user, rolesString);

            return userRolesAddResult.Succeeded ? Result.Successed("User roles updated successfully") : Result.Failure(ApiExeptionType.NotFound, "Error updating user roles");
        }
    }
}