using Savanna.Web.Constants;
using Savanna.Web.Data;
using Savanna.Web.Services.Interfaces;


namespace Savanna.Web.Services
{
    /// <summary>
    /// Service responsible for ensuring database is created
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(
            ApplicationDbContext dbContext,
            ILogger<DatabaseInitializer> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            try
            {
                _logger.LogInformation(WebConstants.EnsuringDbCreatedLogMessage);
                await _dbContext.Database.EnsureCreatedAsync();
                _logger.LogInformation(WebConstants.DbCreationCompleteLogMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, WebConstants.DbCreationErrorLogMessage);
                throw;
            }
        }
    }
}