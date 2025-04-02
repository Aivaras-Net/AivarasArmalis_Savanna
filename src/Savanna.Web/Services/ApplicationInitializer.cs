using Savanna.Web.Constants;
using Savanna.Web.Services.Interfaces;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Service responsible for coordinating application initialization tasks
    /// </summary>
    public class ApplicationInitializer : IApplicationInitializer
    {
        private readonly IDatabaseInitializer _databaseInitializer;
        private readonly IIdentityInitializer _identityInitializer;
        private readonly ILogger<ApplicationInitializer> _logger;

        public ApplicationInitializer(
            IDatabaseInitializer databaseInitializer,
            IIdentityInitializer identityInitializer,
            ILogger<ApplicationInitializer> logger)
        {
            _databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(databaseInitializer));
            _identityInitializer = identityInitializer ?? throw new ArgumentNullException(nameof(identityInitializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation(WebConstants.StartingAppInitLogMessage);

                _logger.LogInformation("Starting database initialization...");
                await _databaseInitializer.EnsureDatabaseCreatedAsync();
                _logger.LogInformation("Database initialization completed successfully");

                _logger.LogInformation("Starting roles initialization...");
                await _identityInitializer.InitializeRolesAsync();
                _logger.LogInformation("Roles initialization completed successfully");

                _logger.LogInformation("Starting default users initialization...");
                await _identityInitializer.InitializeDefaultUsersAsync();
                _logger.LogInformation("Default users initialization completed successfully");

                _logger.LogInformation(WebConstants.AppInitCompletedLogMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.AppInitErrorLogMessage);
                throw;
            }
        }
    }
}