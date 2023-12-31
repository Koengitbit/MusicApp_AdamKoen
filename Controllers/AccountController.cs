using Microsoft.AspNetCore.Mvc;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers
{
    public class AccountController : Controller
    {
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password, // Consider hashing the password
                    BirthDate = model.BirthDate
                };

                // Add user registration logic here (e.g., saving to database)

                return RedirectToAction("Index", "Home"); // Redirect as appropriate
            }

            return View(model);
        }
    }
}
