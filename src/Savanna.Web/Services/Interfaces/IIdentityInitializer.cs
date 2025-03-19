namespace Savanna.Web.Services.Interfaces
{
    /// <summary>
    /// Interface for initializing identity roles and default users
    /// </summary>
    public interface IIdentityInitializer
    {
        /// <summary>
        /// Initializes roles in the system
        /// </summary>
        Task InitializeRolesAsync();

        /// <summary>
        /// Initializes default users like admin
        /// </summary>
        Task InitializeDefaultUsersAsync();
    }
}