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
                _logger.LogInformation("Ensuring database is created");
                await _dbContext.Database.EnsureCreatedAsync();
                _logger.LogInformation("Database creation check completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while ensuring database is created");
                throw;
            }
        }
    }
}