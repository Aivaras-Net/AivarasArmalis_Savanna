using Savanna.Web.Components;
using Savanna.Web.Data;
using Savanna.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Savanna.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddScoped<ApplicationDbContext>(provider =>
                provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/AccessDenied";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapControllers();

            // Create the database if it doesn't exist and initialize roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();

                    // Initialize roles
                    InitializeRolesAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database initialization.");
                }
            }

            app.Run();
        }

        private static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Created role: {roleName}");
                    }
                    else
                    {
                        logger.LogError($"Failed to create role {roleName}: {string.Join(", ", result.Errors)}");
                    }
                }
            }

            var adminEmail = "admin@test.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var password = "Admin123!";
                var createResult = await userManager.CreateAsync(adminUser, password);

                if (createResult.Succeeded)
                {
                    logger.LogInformation($"Created admin user with email: {adminEmail}");

                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("Assigned Admin role to admin user");

                        var verifyUser = await userManager.FindByEmailAsync(adminEmail);
                        if (verifyUser != null)
                        {
                            var isInRole = await userManager.IsInRoleAsync(verifyUser, "Admin");
                            logger.LogInformation($"Admin user verification - Found: true, Has Admin role: {isInRole}");
                        }
                        else
                        {
                            logger.LogError("Admin user verification failed - user not found");
                        }
                    }
                    else
                    {
                        logger.LogError($"Failed to assign Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }
                else
                {
                    logger.LogError($"Failed to create admin user: {string.Join(", ", createResult.Errors)}");
                }
            }
            else
            {
                var isInAdminRole = await userManager.IsInRoleAsync(adminUser, "Admin");
                if (!isInAdminRole)
                {
                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("Assigned Admin role to existing admin user");
                    }
                    else
                    {
                        logger.LogError($"Failed to assign Admin role: {string.Join(", ", roleResult.Errors)}");
                    }
                }

                logger.LogInformation($"Existing admin user found - Email: {adminUser.Email}, Has Admin role: {isInAdminRole}");
            }
        }
    }
}
