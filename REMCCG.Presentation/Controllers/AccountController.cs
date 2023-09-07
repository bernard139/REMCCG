using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using REMCCG.Application.Common.Models;
using REMCCG.Domain.Entities;
using REMCCG.Application.Interfaces.UserAccounts;
using REMCCG.Application.Implementations.UserAccount;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRegister _accountRegister;
        private readonly IAccountLogin _accountLogin;
        private readonly IAccountLogout _accountLogout;
        public AccountController(IAccountLogin accountLogin, IAccountRegister accountRegister, IAccountLogout accountLogout)
        {
            _accountLogin = accountLogin;
            _accountRegister = accountRegister;
            _accountLogout = accountLogout;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRegister.RegisterAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var loggedIn = await _accountLogin.LoginAsync(model);

                if (loggedIn)
                {

                    return RedirectToAction("Index", "Home"); // Redirect to the home page
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Call your LogoutAsync service method here
            await _accountLogout.LogoutAsync(HttpContext);

            // Redirect to a page after logout
            return RedirectToAction("Index", "Home");
        }
    }
}
