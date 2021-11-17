using System;

namespace NiuX.DependencyInjection.Abstractions
{
    public interface IServiceContainer : IScope, IServiceProvider
    {
        IServiceContainer CreateScope();
    }
}