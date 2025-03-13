using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Savanna.Web.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("ProcessLogin")]
        public async Task<IActionResult> ProcessLogin(string email, string password, bool rememberMe, string returnUrl = null)
        {
            // This doesn't count login failures towards account lockout
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }
            else if (result.RequiresTwoFactor)
            {
                return RedirectToPage("/Account/LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            else if (result.IsLockedOut)
            {
                return RedirectToPage("/Account/Lockout");
            }
            else
            {
                return Redirect("/Account/Login?error=Invalid+login+attempt");
            }
        }

        [HttpPost("ProcessRegister")]
        public async Task<IActionResult> ProcessRegister(
            string firstName,
            string lastName,
            string email,
            string password,
            string confirmPassword)
        {
            // Simple validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Redirect("/Account/Register?error=All+fields+are+required");
            }

            if (password != confirmPassword)
            {
                return Redirect("/Account/Register?error=The+password+and+confirmation+password+do+not+match");
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/");
                }
                else
                {
                    string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Redirect($"/Account/Register?error={Uri.EscapeDataString(errorMessage)}");
                }
            }
            catch (Exception ex)
            {
                return Redirect($"/Account/Register?error={Uri.EscapeDataString(ex.Message)}");
            }
        }
    }
}