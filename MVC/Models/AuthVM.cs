namespace MVC.Models
{
    public class AuthVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public string token { get; set; }
    }
}
