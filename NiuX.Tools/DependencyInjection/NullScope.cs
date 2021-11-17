using NiuX.DependencyInjection.Abstractions;

namespace NiuX.DependencyInjection
{
    public class NullScope : IScope
    {
        public void Dispose()
        {
        }

        public static NullScope Instance { get; } = new();
    }
}