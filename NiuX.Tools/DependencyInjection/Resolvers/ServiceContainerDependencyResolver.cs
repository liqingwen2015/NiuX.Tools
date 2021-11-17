using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection.Resolvers
{
    public static partial class DependencyResolver
    {
        private class ServiceContainerDependencyResolver : IDependencyResolver
        {
            private readonly IServiceContainer _rootContainer;

            public ServiceContainerDependencyResolver(IServiceContainer serviceContainer) => _rootContainer = serviceContainer;

            public object GetService(Type serviceType) => _rootContainer.GetService(serviceType);

            public IEnumerable<object> GetServices(Type serviceType) => (IEnumerable<object>)_rootContainer.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType));

            public bool TryInvokeService<TService>(Action<TService>? action)
            {
                if (action == null)
                {
                    return false;
                }

                using var scope = _rootContainer.CreateScope();
                var svc = (TService)scope.GetService(typeof(TService));
                if (svc == null)
                {
                    return false;
                }
                action.Invoke(svc);
                return true;
            }

            public async Task<bool> TryInvokeServiceAsync<TService>(Func<TService, Task> action)
            {
                Guard.NotNull(action, nameof(action));

                using var scope = _rootContainer.CreateScope();
                var svc = (TService)scope.GetService(typeof(TService));
                if (svc == null)
                {
                    return false;
                }
                await action.Invoke(svc);
                return true;
            }
        }
    }
}
