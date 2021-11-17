using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NiuX.DependencyInjection.Abstractions
{
    /// <inheritdoc />
    /// <summary>
    /// IDependencyResolver
    /// </summary>
    public interface IDependencyResolver : IServiceProvider
    {
        /// <summary>
        /// GetServices
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        /// Invoke action via get a service instance internal
        /// </summary>
        /// <typeparam name="TService">service type</typeparam>
        /// <param name="action">action</param>
        bool TryInvokeService<TService>(Action<TService> action);

        Task<bool> TryInvokeServiceAsync<TService>(Func<TService, Task> action);
    }
}
