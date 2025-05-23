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
using Savanna.Web.Constants;

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
                var adminEmail = Environment.GetEnvironmentVariable(WebConstants.AdminEmailEnvVar);
                var adminUsername = Environment.GetEnvironmentVariable(WebConstants.AdminUsernameEnvVar);
                var adminPassword = Environment.GetEnvironmentVariable(WebConstants.AdminPasswordEnvVar);

                if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword))
                {
                    throw new InvalidOperationException(WebConstants.AdminSettingsNotFoundMessage);
                }

                options.Email = adminEmail;
                options.Username = adminUsername;
                options.Password = adminPassword;
            });

            var connectionString = builder.Configuration.GetConnectionString(WebConstants.DefaultConnectionString) ??
                throw new InvalidOperationException(WebConstants.ConnectionStringNotFoundMessage);
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddScoped<ApplicationDbContext>(provider =>
                provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                builder.Configuration.GetSection(WebConstants.IdentitySignInConfigPath).Bind(options.SignIn);
                builder.Configuration.GetSection(WebConstants.IdentityPasswordConfigPath).Bind(options.Password);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = WebConstants.LoginPath;
                options.LogoutPath = WebConstants.LogoutPath;
                options.AccessDeniedPath = WebConstants.AccessDeniedPath;
            });

            builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddScoped<IIdentityInitializer, IdentityInitializer>();
            builder.Services.AddScoped<IApplicationInitializer, ApplicationInitializer>();

            builder.Services.AddSingleton<IGameRenderer, WebGameRenderer>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddSingleton<IAnimalFactory, AnimalFactory>();
            builder.Services.AddSingleton<IPluginService, PluginService>();
            builder.Services.AddScoped<IGameSaveService, GameSaveService>();
            builder.Services.AddHttpClient();

            try
            {
                Savanna.Core.Config.ConfigurationService.LoadConfig();
                Savanna.Core.Config.ConfigurationBootstrap.Initialize();
            }
            catch (Exception ex)
            {
                var loggerFactory = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, WebConstants.ErrorLoadingAnimalConfigMessage);
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(WebConstants.ErrorPath);
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

                    logger.LogInformation(WebConstants.AdminSettingsLoadedMessage);
                    logger.LogInformation(string.Format(WebConstants.AdminEmailLogFormat, adminSettings.Email));
                    logger.LogInformation(string.Format(WebConstants.AdminUsernameLogFormat, adminSettings.Username));
                    logger.LogInformation(string.Format(WebConstants.AdminPasswordMaskedLogFormat, adminSettings.Password.Substring(0, 3)));

                    var initializer = services.GetRequiredService<IApplicationInitializer>();
                    initializer.InitializeAsync().Wait();

                    var pluginService = services.GetRequiredService<IPluginService>();
                    var pluginsFolder = Path.Combine(app.Environment.ContentRootPath, WebConstants.PluginsDirectory);
                    logger.LogInformation("Loading plugins from {PluginsFolder}", pluginsFolder);
                    pluginService.LoadPlugins(pluginsFolder);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, WebConstants.ApplicationInitErrorMessage);
                }
            }

            app.Run();
        }
    }
}
