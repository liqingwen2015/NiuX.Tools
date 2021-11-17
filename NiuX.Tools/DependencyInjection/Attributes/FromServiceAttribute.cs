using System;

namespace NiuX.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class FromServiceAttribute : Attribute
    {
    }
}
