using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Auth(AuthVM authData)
        {
            var response = await _authService.Auth(authData);
            if (response)
                return RedirectToAction("AdminIndex", "Login");

            ModelState.AddModelError(string.Empty, "Incorrect username or password");
            TempData["ErrorMessage"] = "Incorrect username or password";
            return RedirectToAction("Index", "Login");
        }
        public async Task<IActionResult> AdminIndex()
        {
            return View();
        }
    }
}
