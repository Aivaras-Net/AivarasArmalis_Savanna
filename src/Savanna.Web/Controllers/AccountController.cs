using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Constants;
using Savanna.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;

namespace Savanna.Web.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAntiforgery _antiforgery;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAntiforgery antiforgery)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _antiforgery = antiforgery;
        }

        [HttpPost("ProcessLogin")]
        public async Task<IActionResult> ProcessLogin(string email, string password, string? returnUrl, bool rememberMe)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(email);

                    if (user == null)
                    {
                        return Redirect($"/Account/Login?error={Uri.EscapeDataString(WebConstants.InvalidLoginAttemptMessage)}");
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl ?? WebConstants.DefaultReturnPath);
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("/Account/LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = rememberMe });
                }
                else if (result.IsLockedOut)
                {
                    return Redirect("/Account/Lockout");
                }
                else
                {
                    return Redirect($"/Account/Login?error={Uri.EscapeDataString(WebConstants.InvalidPasswordMessage)}");
                }
            }
            catch (Exception ex)
            {
                return Redirect($"/Account/Login?error={Uri.EscapeDataString(string.Format(WebConstants.ErrorOccurredMessage, ex.Message))}");
            }
        }

        [HttpPost("ProcessRegister")]
        public async Task<IActionResult> ProcessRegister(string email, string username, string password, string confirmPassword)
        {
            // Simple validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                return Redirect($"/Account/Register?error={Uri.EscapeDataString(WebConstants.AllFieldsRequiredMessage)}");
            }

            if (password != confirmPassword)
            {
                return Redirect($"/Account/Register?error={Uri.EscapeDataString(WebConstants.PasswordMismatchMessage)}");
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(WebConstants.UserRoleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(WebConstants.UserRoleName));
                    }

                    await _userManager.AddToRoleAsync(user, WebConstants.UserRoleName);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(WebConstants.DefaultReturnPath);
                }
                else
                {
                    string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Redirect($"/Account/Register?error={Uri.EscapeDataString(errorMessage)}");
                }
            }
            catch (Exception ex)
            {
                return Redirect($"/Account/Register?error={Uri.EscapeDataString(string.Format(WebConstants.ErrorOccurredMessage, ex.Message))}");
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect(WebConstants.DefaultReturnPath);
        }
    }
}