using MVC.Models;

namespace MVC.Contracts
{
    public interface IAuthService
    {
        Task<bool> Auth(AuthVM authData);
    }
}
