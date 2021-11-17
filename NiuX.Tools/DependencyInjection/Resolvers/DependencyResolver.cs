using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NiuX.DependencyInjection.Abstractions;
using NiuX.DependencyInjection.Extensions;

namespace NiuX.DependencyInjection.Resolvers
{
    /// <summary>
    /// DependencyResolver
    /// </summary>
    public static partial class DependencyResolver
    {
        public static IDependencyResolver Current { get; private set; } = new DefaultDependencyResolver();

        /// <summary>
        /// locker
        /// </summary>
        private static readonly object Locker = new();

        public static TService ResolveService<TService>() => Current.ResolveService<TService>();

        public static IEnumerable<TService> ResolveServices<TService>() => Current.ResolveServices<TService>();

        public static bool TryInvoke<TService>(Action<TService> action) => Current.TryInvokeService(action);

        public static Task<bool> TryInvokeAsync<TService>(Func<TService, Task> action) => Current.TryInvokeServiceAsync(action);

        public static void SetDependencyResolver([NotNull] IDependencyResolver dependencyResolver)
        {
            lock (Locker)
            {
                Current = dependencyResolver;
            }
        }

        public static void SetDependencyResolver([NotNull] IServiceContainer serviceContainer) => SetDependencyResolver(new ServiceContainerDependencyResolver(serviceContainer));

        public static void SetDependencyResolver([NotNull] IServiceProvider serviceProvider) => SetDependencyResolver(serviceProvider.GetService);

        public static void SetDependencyResolver([NotNull] Func<Type, object> getServiceFunc) => SetDependencyResolver(getServiceFunc, serviceType => (IEnumerable<object>)getServiceFunc(typeof(IEnumerable<>).MakeGenericType(serviceType)));

        public static void SetDependencyResolver([NotNull] Func<Type, object> getServiceFunc, [NotNull] Func<Type, IEnumerable<object>> getServicesFunc) => SetDependencyResolver(new DelegateBasedDependencyResolver(getServiceFunc, getServicesFunc));
    }
}
