using Microsoft.AspNetCore.Mvc;
using MultiVender.Application.DTOs;
using MultiVender.Application.IServices;

namespace MultiVender.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ================= REGISTER =================


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _authService.RegisterAsync(dto);

            if (user == null)
            {
                ModelState.AddModelError("", "Username already exists");
                return View(dto);
            }

            return RedirectToAction("Login");
        }
        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var tokenResponse = await _authService.LoginAsync(dto);

            if (tokenResponse == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(dto);
            }

            // Save JWT in cookie
            Response.Cookies.Append("jwt", tokenResponse.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true in production
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("UserProfile", "ApplyVendor");
        }

        // ================= LOGOUT =================
        
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }
    }
}
