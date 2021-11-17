using System;
using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection.Extensions
{
    /// <summary>
    /// DependencyResolverExtensions
    /// </summary>
    public static class DependencyResolverExtensions
    {
        /// <summary>
        /// TryGetService
        /// </summary>
        /// <param name="dependencyResolver">dependencyResolver</param>
        /// <param name="serviceType">serviceType</param>
        /// <param name="service">service</param>
        /// <returns>true if successfully get service otherwise false</returns>
        public static bool TryGetService(this IDependencyResolver dependencyResolver, Type serviceType, out object? service)
        {
            try
            {
                service = dependencyResolver.GetService(serviceType);
                return service != null;
            }
            catch (Exception e)
            {
                service = null;
                // TODO: log
                // InvokeHelper.OnInvokeException?.Invoke(e);
                return false;
            }
        }

        public static bool TryResolveService<TService>(this IDependencyResolver dependencyResolver,
            out TService? service)
        {
            var result = dependencyResolver.TryGetService(typeof(TService), out var serviceObj);
            if (result)
            {
                service = (TService)serviceObj!;
            }
            else
            {
                service = default;
            }
            return result;
        }
    }
}