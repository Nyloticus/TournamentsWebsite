using Microsoft.AspNetCore.Authorization;

namespace Common
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {

        public AuthorizationAttribute()
        {
            this.AuthenticationSchemes = "Bearer";
        }
    }
}
