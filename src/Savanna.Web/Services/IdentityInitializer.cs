using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Savanna.Web.Configuration;
using Savanna.Web.Constants;
using Savanna.Web.Models;
using Savanna.Web.Services.Interfaces;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Service responsible for initializing identity roles and default users
    /// </summary>
    public class IdentityInitializer : IIdentityInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityInitializer> _logger;
        private readonly AdminSettings _adminSettings;

        public IdentityInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IOptions<AdminSettings> adminSettings,
            ILogger<IdentityInitializer> logger)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adminSettings = adminSettings.Value ?? throw new ArgumentNullException(nameof(adminSettings));
        }

        public async Task InitializeRolesAsync()
        {
            string[] roleNames = { WebConstants.AdminRoleName, WebConstants.UserRoleName };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(string.Format(WebConstants.CreatedRoleLogMessage, roleName));
                    }
                    else
                    {
                        _logger.LogError(string.Format(WebConstants.FailedCreateRoleLogMessage, roleName, string.Join(", ", result.Errors)));
                    }
                }
            }
        }

        public async Task InitializeDefaultUsersAsync()
        {
            await CreateAdminUserIfNotExistsAsync();
        }

        private async Task CreateAdminUserIfNotExistsAsync()
        {
            var adminEmail = _adminSettings.Email;
            _logger.LogInformation(string.Format(WebConstants.CreatingAdminUserMessage, adminEmail));

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = _adminSettings.Username,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var password = _adminSettings.Password;
                var createResult = await _userManager.CreateAsync(adminUser, password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation(string.Format(WebConstants.CreatedAdminUserLogMessage, adminEmail));

                    var roleResult = await _userManager.AddToRoleAsync(adminUser, WebConstants.AdminRoleName);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation(WebConstants.AssignedAdminRoleLogMessage);

                        var verifyUser = await _userManager.FindByEmailAsync(adminEmail);
                        if (verifyUser != null)
                        {
                            var isInRole = await _userManager.IsInRoleAsync(verifyUser, WebConstants.AdminRoleName);
                            _logger.LogInformation(string.Format(WebConstants.AdminUserVerificationLogMessage, isInRole));
                        }
                        else
                        {
                            _logger.LogError(WebConstants.AdminUserVerificationFailedLogMessage);
                        }
                    }
                    else
                    {
                        _logger.LogError(string.Format(WebConstants.FailedAssignRoleLogMessage, string.Join(", ", roleResult.Errors)));
                    }
                }
                else
                {
                    _logger.LogError(string.Format(WebConstants.FailedCreateUserLogMessage, string.Join(", ", createResult.Errors)));
                }
            }
            else
            {
                var isInAdminRole = await _userManager.IsInRoleAsync(adminUser, WebConstants.AdminRoleName);
                if (!isInAdminRole)
                {
                    var roleResult = await _userManager.AddToRoleAsync(adminUser, WebConstants.AdminRoleName);
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation(WebConstants.AssignedAdminRoleExistingUserLogMessage);
                    }
                    else
                    {
                        _logger.LogError(string.Format(WebConstants.FailedAssignRoleLogMessage, string.Join(", ", roleResult.Errors)));
                    }
                }

                _logger.LogInformation(string.Format(WebConstants.ExistingAdminUserLogMessage, adminUser.Email, isInAdminRole));
            }
        }
    }
}