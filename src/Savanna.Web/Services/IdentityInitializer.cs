using Microsoft.AspNetCore.Identity;
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

        public IdentityInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<IdentityInitializer> logger)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitializeRolesAsync()
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Created role: {roleName}");
                    }
                    else
                    {
                        _logger.LogError($"Failed to create role {roleName}: {string.Join(", ", result.Errors)}");
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
            var adminEmail = "admin@test.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var password = "Admin123!";
                var createResult = await _userManager.CreateAsync(adminUser, password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation($"Created admin user with email: {adminEmail}");

                    var roleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Assigned Admin role to admin user");

                        var verifyUser = await _userManager.FindByEmailAsync(adminEmail);
                        if (verifyUser != null)
                        {
                            var isInRole = await _userManager.IsInRoleAsync(verifyUser, "Admin");
                            _logger.LogInformation($"Admin user verification - Found: true, Has Admin role: {isInRole}");
                        }
                        else
                        {
                            _logger.LogError("Admin user verification failed - user not found");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Failed to assign Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to create admin user: {string.Join(", ", createResult.Errors)}");
                }
            }
            else
            {
                var isInAdminRole = await _userManager.IsInRoleAsync(adminUser, "Admin");
                if (!isInAdminRole)
                {
                    var roleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("Assigned Admin role to existing admin user");
                    }
                    else
                    {
                        _logger.LogError($"Failed to assign Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }

                _logger.LogInformation($"Existing admin user found - Email: {adminUser.Email}, Has Admin role: {isInAdminRole}");
            }
        }
    }
}