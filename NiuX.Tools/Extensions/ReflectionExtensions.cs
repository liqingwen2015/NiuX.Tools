using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using NiuX.Helpers;

namespace NiuX.Extensions
{
    public static class ReflectionExtensions
    {
        public static Func<T, object?>? GetValueGetter<T>(this PropertyInfo propertyInfo)
        {
            return StrongTypedCache<T>.PropertyValueGetters.GetOrAdd(propertyInfo, prop =>
            {
                if (!prop.CanRead)
                    return null;

                var instance = Expression.Parameter(typeof(T), "i");
                var property = Expression.Property(instance, prop);
                var convert = Expression.TypeAs(property, typeof(object));
                return (Func<T, object>)Expression.Lambda(convert, instance).Compile();
            });
        }

        public static Func<object, object?>? GetValueGetter([NotNull] this PropertyInfo propertyInfo)
        {
            return TypeCacheHelper.PropertyValueGetters.GetOrAdd(propertyInfo, prop =>
            {
                if (!prop.CanRead)
                    return null;

                Debug.Assert(propertyInfo.DeclaringType != null);

                var instance = Expression.Parameter(typeof(object), "obj");
                var getterCall = Expression.Call(propertyInfo.DeclaringType!.IsValueType
                    ? Expression.Unbox(instance, propertyInfo.DeclaringType)
                    : Expression.Convert(instance, propertyInfo.DeclaringType), prop.GetGetMethod());
                var castToObject = Expression.Convert(getterCall, typeof(object));
                return (Func<object, object>)Expression.Lambda(castToObject, instance).Compile();
            });
        }

        public static Action<T, object?>? GetValueSetter<T>(this PropertyInfo propertyInfo) where T : class
        {
            return StrongTypedCache<T>.PropertyValueSetters.GetOrAdd(propertyInfo, prop =>
            {
                if (!prop.CanWrite)
                    return null;

                var instance = Expression.Parameter(typeof(T), "i");
                var argument = Expression.Parameter(typeof(object), "a");
                var setterCall = Expression.Call(instance, prop.GetSetMethod(), Expression.Convert(argument, prop.PropertyType));
                return (Action<T, object?>)Expression.Lambda(setterCall, instance, argument).Compile();
            });
        }

        public static Action<object, object?>? GetValueSetter(this PropertyInfo propertyInfo)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            return TypeCacheHelper.PropertyValueSetters.GetOrAdd(propertyInfo, prop =>
            {
                if (!prop.CanWrite)
                    return null;

                var obj = Expression.Parameter(typeof(object), "o");
                var value = Expression.Parameter(typeof(object));

                // Note that we are using Expression.Unbox for value types and Expression.Convert for reference types
                var expr =
                    Expression.Lambda<Action<object, object?>>(
                        Expression.Call(
                            propertyInfo.DeclaringType!.IsValueType
                                ? Expression.Unbox(obj, propertyInfo.DeclaringType)
                                : Expression.Convert(obj, propertyInfo.DeclaringType),
                            propertyInfo.GetSetMethod(),
                            Expression.Convert(value, propertyInfo.PropertyType)),
                        obj, value);
                return expr.Compile();
            });
        }
    }
}