using System;

namespace NiuX.DependencyInjection
{
    /// <summary>
    /// 服务容器
    /// </summary>
    internal sealed partial class ServiceContainer
    {
        /// <summary>
        /// 键
        /// </summary>
        private class ServiceKey : IEquatable<ServiceKey>
        {
            /// <summary>
            /// 服务类型
            /// </summary>
            public Type ServiceType { get; }

            /// <summary>
            /// 实现类型
            /// </summary>
            public Type ImplementType { get; }

            public ServiceKey(Type serviceType, ServiceDefinition definition)
            {
                ServiceType = serviceType;
                ImplementType = definition.GetImplementType();
            }

            public bool Equals(ServiceKey? other) => ServiceType == other?.ServiceType && ImplementType == other.ImplementType;

            public override bool Equals(object obj) => Equals((ServiceKey?)obj);

            public override int GetHashCode() => (ServiceType.FullName + "_" + ImplementType.FullName).GetHashCode();
        }
    }
}
