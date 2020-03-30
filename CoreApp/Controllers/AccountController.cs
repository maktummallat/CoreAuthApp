using CoreApp.Data.Entities;
using CoreApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreApp.Controllers
{
    public class AccountController: Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.EmailAddress);

            if (user == null)
            {
                // email user
            }
            else
            {
                var token = await userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                var url = Url.Action("LoginCallback", "Account", new { token = token, email = model.EmailAddress }, Request.Scheme);
                System.IO.File.WriteAllText("passwordless.txt", url);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LoginCallback(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var isValid = await userManager.VerifyUserTokenAsync(user, "Default", "passwordless-auth", token);

            if (isValid)
            {
                await userManager.UpdateSecurityStampAsync(user);

                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(
                        new List<Claim> { new Claim("sub", user.Id) },
                        IdentityConstants.ApplicationScheme)));
                return RedirectToAction("Index", "Home");
            }

            return View("Error");
        }
    }
}
