using Savanna.Web.Components;
using Savanna.Web.Data;
using Savanna.Web.Models;
using Savanna.Web.Services;
using Savanna.Web.Services.Interfaces;
using Savanna.Core.Interfaces;
using Savanna.Core.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Savanna.Web.Configuration;
using Microsoft.Extensions.Options;

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

            DotNetEnv.Env.Load();

            builder.Services.Configure<AdminSettings>(options =>
            {
                var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
                var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
                var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

                if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword))
                {
                    throw new InvalidOperationException("Admin settings not found in .env file. Please create a .env file with ADMIN_EMAIL, ADMIN_USERNAME, and ADMIN_PASSWORD.");
                }

                options.Email = adminEmail;
                options.Username = adminUsername;
                options.Password = adminPassword;
            });

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

            builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddScoped<IIdentityInitializer, IdentityInitializer>();
            builder.Services.AddScoped<IApplicationInitializer, ApplicationInitializer>();

            builder.Services.AddSingleton<IGameRenderer, WebGameRenderer>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddSingleton<IAnimalFactory, AnimalFactory>();

            try
            {
                Savanna.Core.Config.ConfigurationService.LoadConfig();
            }
            catch (Exception ex)
            {
                var loggerFactory = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "Error loading animal configuration");
            }

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var adminSettings = services.GetRequiredService<IOptions<AdminSettings>>().Value;

                    logger.LogInformation("Admin settings loaded from .env file:");
                    logger.LogInformation($"  Email: {adminSettings.Email}");
                    logger.LogInformation($"  Username: {adminSettings.Username}");
                    logger.LogInformation($"  Password: {adminSettings.Password.Substring(0, 3)}*** (masked)");

                    var initializer = services.GetRequiredService<IApplicationInitializer>();
                    initializer.InitializeAsync().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during application initialization.");
                }
            }

            app.Run();
        }
    }
}
