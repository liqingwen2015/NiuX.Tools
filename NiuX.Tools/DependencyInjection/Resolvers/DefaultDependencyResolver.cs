using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection.Resolvers
{
    public static partial class DependencyResolver
    {
        private class DefaultDependencyResolver : IDependencyResolver
        {
            public object? GetService(Type serviceType)
            {
                // Since attempting to create an instance of an interface or an abstract type results in an exception, immediately return null
                // to improve performance and the debugging experience with first-chance exceptions enabled.
                if (serviceType.IsInterface || serviceType.IsAbstract)
                {
                    return null;
                }
                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }

            public IEnumerable<object> GetServices(Type serviceType) => Enumerable.Empty<object>();

            public bool TryInvokeService<TService>(Action<TService>? action)
            {
                var service = GetService(typeof(TService));
                if (service == null || action == null)
                {
                    return false;
                }
                action.Invoke((TService)service);
                return true;
            }

            public async Task<bool> TryInvokeServiceAsync<TService>(Func<TService, Task>? action)
            {
                var service = GetService(typeof(TService));
                if (service == null || action == null)
                {
                    return false;
                }
                await action.Invoke((TService)service);
                return true;
            }
        }
    }
}
