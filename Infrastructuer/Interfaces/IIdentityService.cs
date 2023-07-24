using Common;
using Domain.Entities.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(string firstName, string lastName, string email, string phoneNumber, string userName, string password, List<string> RolesIds);
        Task<Result> LoginAsync(string email, string password, string deviceToken);
        Task<Result> LogoutAsync(string deviceToken);
        Task<Result> ValidateUserPassword(string userId, string password);
        Task<Result> RefreshTokenAsync(string token, string refreshToken);

        Task<Result> ChangePasswordAsync(string password, string newPassword);
        Task<Result> ForgetPasswordAsync(string email);
        Task<Result> ResetPasswordAsync(string token, string email, string password);
        Task<Result> GenerateAuthenticationResultForUserAsync(User user);
        Task<Result> RemoveUserAsync(string userId);
        Task<Result> GetActiveUsers();
        Task<Result> UpdateUserRoles(string userId, List<string> RolesIds);
    }
}