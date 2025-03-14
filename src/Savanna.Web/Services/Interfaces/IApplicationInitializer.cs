namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Interface for coordinating application startup initialization
    /// </summary>
    public interface IApplicationInitializer
    {
        /// <summary>
        /// Initializes the application with all required setup tasks
        /// </summary>
        Task InitializeAsync();
    }
}