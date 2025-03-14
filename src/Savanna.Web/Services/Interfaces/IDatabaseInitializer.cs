namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Interface for initializing database and ensuring it exists
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ensures the database exists and is created
        /// </summary>
        Task EnsureDatabaseCreatedAsync();
    }
}