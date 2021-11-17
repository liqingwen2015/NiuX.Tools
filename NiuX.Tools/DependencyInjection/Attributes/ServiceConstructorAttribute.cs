using System;

namespace NiuX.DependencyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class ServiceConstructorAttribute : Attribute
    {
    }
}
