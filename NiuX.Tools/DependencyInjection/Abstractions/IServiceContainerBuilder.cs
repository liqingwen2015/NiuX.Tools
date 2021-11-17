using System.Collections.Generic;

namespace NiuX.DependencyInjection.Abstractions
{
    public interface IServiceContainerBuilder : IEnumerable<ServiceDefinition>
    {
        IServiceContainerBuilder Add(ServiceDefinition item);

        IServiceContainerBuilder TryAdd(ServiceDefinition item);

        IServiceContainer Build();
    }
}