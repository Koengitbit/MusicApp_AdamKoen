using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly SpotifyDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;
        public AccountController(SpotifyDbContext db, PasswordHasher<User> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //can remove this probably we'll see if it's useful later or remove
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home");
                    
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    BirthDate = model.BirthDate
                };
                user.Password = _passwordHasher.HashPassword(user, model.Password);
                
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
