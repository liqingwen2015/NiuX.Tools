using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection
{
    public interface IServiceContainerModule
    {
        void ConfigureServices(IServiceContainerBuilder serviceContainerBuilder);
    }

    public abstract class ServiceContainerModule : IServiceContainerModule
    {
        public abstract void ConfigureServices(IServiceContainerBuilder serviceContainerBuilder);
    }
}
