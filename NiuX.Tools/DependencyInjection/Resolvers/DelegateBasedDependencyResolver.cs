using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection.Resolvers
{
    public static partial class DependencyResolver
    {
        private class DelegateBasedDependencyResolver : IDependencyResolver
        {
            private readonly Func<Type, object> _getService;
            private readonly Func<Type, IEnumerable<object>> _getServices;

            public DelegateBasedDependencyResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
            {
                _getService = getService;
                _getServices = getServices;
            }

            public object GetService(Type serviceType) => _getService(serviceType);

            public IEnumerable<object> GetServices(Type serviceType) => _getServices(serviceType);

            public bool TryInvokeService<TService>(Action<TService>? action)
            {
                var service = (TService)GetService(typeof(TService));
                if (service == null || action == null)
                {
                    return false;
                }
                action.Invoke(service);
                return true;
            }

            public async Task<bool> TryInvokeServiceAsync<TService>(Func<TService, Task>? action)
            {
                var service = (TService)GetService(typeof(TService));
                if (null == service || action == null)
                {
                    return false;
                }
                await action.Invoke(service);
                return true;
            }
        }
    }
}
