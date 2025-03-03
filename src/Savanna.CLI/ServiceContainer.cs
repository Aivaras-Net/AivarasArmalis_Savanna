using Savanna.CLI.Interfaces;
using Savanna.CLI.Services;
using Savanna.Core.Constants;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
    /// <summary>
    /// Simple service container for dependency injection
    /// </summary>
    public class ServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers all required services
        /// </summary>
        public void RegisterServices()
        {
            RegisterSingleton<ILogService>(new LogService());

            var logService = GetService<ILogService>();
            var renderer = new RendererService(logService, ConsoleConstants.TotalHeaderOffset);

            RegisterSingleton<IRendererService>(renderer);
            RegisterSingleton<IConsoleRenderer>(renderer);

            RegisterSingleton<IMenuService>(new MenuService(renderer));
        }

        /// <summary>
        /// Registers a singleton service
        /// </summary>
        /// <typeparam name="T">The service interface type</typeparam>
        /// <param name="implementation">The service implementation</param>
        public void RegisterSingleton<T>(object implementation)
        {
            _services[typeof(T)] = implementation;
        }

        /// <summary>
        /// Gets a registered service
        /// </summary>
        /// <typeparam name="T">The service interface type</typeparam>
        /// <returns>The service implementation</returns>
        public T GetService<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            throw new InvalidOperationException(string.Format(ConsoleConstants.ServiceNotRegisteredFormat, typeof(T).Name));
        }
    }
}