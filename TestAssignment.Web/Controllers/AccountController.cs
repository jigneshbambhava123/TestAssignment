using Microsoft.AspNetCore.Mvc;
using TestAssignment.Entity.Helpers;
using TestAssignment.Entity.ViewModels;
using TestAssignment.Service.Interfaces;

namespace TestAssignment.Web.Controllers;

public class AccountController:Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;


    private readonly IAuthService _authService;

      public AccountController(IConfiguration configuration,IAuthService authService,ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _configuration = configuration;

        }

        // GET : Login Page
        public IActionResult Login()
        {
            return View();
        }

        //POST: Login page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var user = await _authService.AuthenticateUserAsync(loginModel.Email,loginModel.Password);

            if (user==null || !PasswordHasher.VerifyPassword(loginModel.Password,user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Invalid email or password"; 
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginModel);
            }
            
            if (loginModel.RememberMe)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(1),
                    IsEssential = true, 
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict 
                };
                Response.Cookies.Append("Email", loginModel.Email, options);
                Response.Cookies.Append("Password", loginModel.Password, options);
            }

            var jwtSettings = _configuration["JwtSettings:Key"];

            if(string.IsNullOrEmpty(jwtSettings))
            {
                return RedirectToAction("Login","Account");
            }

            var token = _tokenService.GenerateToken(
                    user.Username,
                user.Email,
                user.Role.Name,
                loginModel.RememberMe,
                jwtSettings,
                "localhost",
                "localhost",
                user.Id
            );

            Response.Cookies.Append("AuthToken", await token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,  
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            //redirect based on role
            return user.Role.Name switch{
                "Admin" => RedirectToAction("Admin","Home"),
                "User" => RedirectToAction("User","Home"),
                _  =>RedirectToAction("Login","Account")
            };
        }

        public async Task<IActionResult> Logout(){ 
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login","Account");
        }

        [Route("/Account/PageNotFound")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PageNotFound()
        {
            return View("~/Views/Account/PageNotFound.cshtml"); 
        }


}
