using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace NiuX.DependencyInjection.Extensions
{
    /// <summary>
    /// 依赖注入扩展
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        public static IEnumerable<object> GetServices(
            this IServiceProvider provider,
            Type serviceType)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            return (IEnumerable<object>)provider.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType));
        }

        /// <summary>
        /// ResolveService
        /// </summary>
        /// <typeparam name="TService">TService</typeparam>
        /// <param name="serviceProvider">serviceProvider</param>
        /// <returns></returns>
        public static TService ResolveService<TService>([NotNull] this IServiceProvider serviceProvider) => (TService)serviceProvider.GetService(typeof(TService));

        /// <summary>
        /// ResolveRequiredService
        /// throw exception if can not get a service instance
        /// </summary>
        /// <typeparam name="TService">TService</typeparam>
        /// <param name="serviceProvider">serviceProvider</param>
        /// <returns></returns>
        public static TService ResolveRequiredService<TService>([NotNull] this IServiceProvider serviceProvider)
        {
            var serviceType = typeof(TService);
            var svc = serviceProvider.GetService(serviceType);
            if (svc == null)
            {
                throw new InvalidOperationException($"service had not been registered, serviceType: {serviceType}");
            }
            return (TService)svc;
        }

        /// <summary>
        /// Resolve services
        /// </summary>
        /// <typeparam name="TService">TService</typeparam>
        /// <param name="serviceProvider">serviceProvider</param>
        /// <returns></returns>
        public static IEnumerable<TService> ResolveServices<TService>([NotNull] this IServiceProvider serviceProvider) => serviceProvider.ResolveService<IEnumerable<TService>>();
    }
}
