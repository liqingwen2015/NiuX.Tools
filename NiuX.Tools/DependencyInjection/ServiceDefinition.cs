using System;

namespace NiuX.DependencyInjection
{
    /// <summary>
    /// 服务定义
    /// </summary>
    public class ServiceDefinition
    {
        /// <summary>
        /// 服务生命周期
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; }

        /// <summary>
        /// 实现类型
        /// </summary>
        public Type? ImplementType { get; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// 实现实例
        /// </summary>
        public object? ImplementationInstance { get; }

        /// <summary>
        /// 实现工厂
        /// </summary>
        public Func<IServiceProvider, object>? ImplementationFactory { get; }

        /// <summary>
        /// 获取实现类型
        /// </summary>
        /// <returns></returns>
        public Type GetImplementType()
        {
            if (ImplementationInstance != null)
                return ImplementationInstance.GetType();

            if (ImplementationFactory != null)
                return ImplementationFactory.Method.ReturnType;

            if (ImplementType != null)
                return ImplementType;

            return ServiceType;
        }

        #region ctor

        private ServiceDefinition(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) =>
            ServiceLifetime = serviceLifetime;

        public ServiceDefinition(object instance, Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : this(serviceLifetime)
        {
            ImplementationInstance = instance;
            ServiceType = serviceType;
        }

        public ServiceDefinition(Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : this(serviceType, serviceType, serviceLifetime)
        {
        }

        public ServiceDefinition(Type serviceType, Type? implementType, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : this(serviceLifetime)
        {
            ServiceType = serviceType;
            ImplementType = implementType ?? serviceType;
        }

        public ServiceDefinition(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : this(serviceLifetime)
        {
            ServiceType = serviceType;
            ImplementationFactory = factory;
        }

        #endregion

        public static ServiceDefinition CreateSingleton<TService>(Func<IServiceProvider, object> factory) => new(typeof(TService), factory);

        public static ServiceDefinition CreateSingleton<TService, TServiceImplement>() where TServiceImplement : TService => new(typeof(TService), typeof(TServiceImplement));

        public static ServiceDefinition CreateSingleton<TService>() => new(typeof(TService));

        public static ServiceDefinition CreateScoped<TService>(Func<IServiceProvider, object> factory) => new(typeof(TService), factory, ServiceLifetime.Scoped);

        public static ServiceDefinition CreateScoped<TService, TServiceImplement>() where TServiceImplement : TService => new(typeof(TService), typeof(TServiceImplement), ServiceLifetime.Scoped);

        public static ServiceDefinition CreateScoped<TService>() => new(typeof(TService), ServiceLifetime.Scoped);

        public static ServiceDefinition CreateTransient<TService>(Func<IServiceProvider, object> factory) => new(typeof(TService), factory, ServiceLifetime.Transient);

        public static ServiceDefinition CreateTransient<TService>() => new(typeof(TService), ServiceLifetime.Transient);

        public static ServiceDefinition CreateTransient<TService, TServiceImplement>() where TServiceImplement : TService => new(typeof(TService), typeof(TServiceImplement), ServiceLifetime.Transient);
    }
}
