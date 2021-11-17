using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NiuX;

namespace System
{
    public static partial class NiuXTypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
        {
            Checker.NotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// Determines whether an instance of this type can be assigned to
        /// an instance of the <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo([NotNull] this Type type, [NotNull] Type targetType)
        {
            Checker.NotNull(type, nameof(type));
            Checker.NotNull(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, bool includeObject = true)
        {
            Checker.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        /// <summary>
        /// Gets all base classes of this type.
        /// </summary>
        /// <param name="type">The type to get its base classes.</param>
        /// <param name="stoppingType">A type to stop going to the deeper base classes. This type will be be included in the returned array</param>
        /// <param name="includeObject">True, to include the standard <see cref="object"/> type in the returned array.</param>
        public static Type[] GetBaseClasses([NotNull] this Type type, Type stoppingType, bool includeObject = true)
        {
            Checker.NotNull(type, nameof(type));

            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
            [NotNull] List<Type> types,
            [CanBeNull] Type type,
            bool includeObject,
            [CanBeNull] Type stoppingType = null)
        {
            if (type == null || type == stoppingType)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            types.Add(type);
        }
    }

    public static partial class NiuXTypeExtensions
    {
        private static readonly HashSet<Type> FloatingTypes = new HashSet<Type>
        {
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        private static readonly HashSet<Type> NonNullablePrimitiveTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(bool),
            typeof(float),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        public static bool IsNonNullablePrimitiveType(this Type type)
        {
            return NonNullablePrimitiveTypes.Contains(type);
        }

        public static bool IsFunc(this object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public static bool IsFunc<TReturn>(this object obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }

        public static bool IsPrimitiveExtended(this Type type, bool includeNullables = true, bool includeEnums = false)
        {
            if (IsPrimitiveExtendedInternal(type, includeEnums))
            {
                return true;
            }

            if (includeNullables && IsNullable(type) && type.GenericTypeArguments.Any())
            {
                return IsPrimitiveExtendedInternal(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetFirstGenericArgumentIfNullable(this Type t)
        {
            if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return t.GetGenericArguments().FirstOrDefault();
            }

            return t;
        }

        private static bool IsPrimitiveExtendedInternal(this Type type, bool includeEnums)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }

        public static object GetDefaultValue(this Type type) 
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static string GetFullNameHandlingNullableAndGenerics([NotNull] this Type type)
        {
            Checker.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GenericTypeArguments[0].FullName + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.FullName.Left(genericType.FullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetFullNameHandlingNullableAndGenerics).JoinAsString(",")}>";
            }

            return type.FullName ?? type.Name;
        }

        public static string GetSimplifiedName([NotNull] this Type type)
        {
            Checker.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetSimplifiedName(type.GenericTypeArguments[0]) + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.FullName.Left(genericType.FullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetSimplifiedName).JoinAsString(",")}>";
            }

            if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(int))
            {
                return "number";
            }
            else if (type == typeof(long))
            {
                return "number";
            }
            else if (type == typeof(bool))
            {
                return "boolean";
            }
            else if (type == typeof(char))
            {
                return "string";
            }
            else if (type == typeof(double))
            {
                return "number";
            }
            else if (type == typeof(float))
            {
                return "number";
            }
            else if (type == typeof(decimal))
            {
                return "number";
            }
            else if (type == typeof(DateTime))
            {
                return "string";
            }
            else if (type == typeof(DateTimeOffset))
            {
                return "string";
            }
            else if (type == typeof(TimeSpan))
            {
                return "string";
            }
            else if (type == typeof(Guid))
            {
                return "string";
            }
            else if (type == typeof(byte))
            {
                return "number";
            }
            else if (type == typeof(sbyte))
            {
                return "number";
            }
            else if (type == typeof(short))
            {
                return "number";
            }
            else if (type == typeof(ushort))
            {
                return "number";
            }
            else if (type == typeof(uint))
            {
                return "number";
            }
            else if (type == typeof(ulong))
            {
                return "number";
            }
            else if (type == typeof(IntPtr))
            {
                return "number";
            }
            else if (type == typeof(UIntPtr))
            {
                return "number";
            }
            else if (type == typeof(object))
            {
                return "object";
            }

            return type.FullName ?? type.Name;
        }

        public static bool IsFloatingType(this Type type, bool includeNullable = true)
        {
            if (FloatingTypes.Contains(type))
            {
                return true;
            }

            if (includeNullable &&
                IsNullable(type) &&
                FloatingTypes.Contains(type.GenericTypeArguments[0]))
            {
                return true;
            }

            return false;
        }

        public static object ConvertFrom<TTargetType>(this object value)
        {
            return ConvertFrom(typeof(TTargetType), value);
        }

        public static object ConvertFrom(this Type targetType, object value)
        {
            return TypeDescriptor
                .GetConverter(targetType)
                .ConvertFrom(value);
        }

        public static Type StripNullable(this Type type)
        {
            return IsNullable(type)
                ? type.GenericTypeArguments[0]
                : type;
        }

        public static bool IsDefaultValue([CanBeNull] this object obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Equals(GetDefaultValue(obj.GetType()));
        }

    }


    public static partial class NiuXTypeExtensions
    {
        /// <summary>
        /// 是否可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type) => (((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));

        /// <summary>
        /// 获取不可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNonNullableType(this Type type) => IsNullableType(type) ? type.GetGenericArguments()[0] : type;

        /// <summary>
        /// 是否泛型可枚举类型
        /// </summary>
        /// <param name="enumerableType"></param>
        /// <returns></returns>
        public static bool IsGenericEnumerableType(this Type enumerableType) => (FindGenericType(typeof(IEnumerable<>), enumerableType) != null);

        /// <summary>
        /// 获取泛型元素类型
        /// </summary>
        /// <param name="enumerableType"></param>
        /// <param name="argumentIndex">参数下标</param>
        /// <returns></returns>
        public static Type GetGenericElementType(this Type enumerableType, int argumentIndex = 0)
        {
            var type = FindGenericType(typeof(IEnumerable<>), enumerableType);
            return type != null ? type.GetGenericArguments()[argumentIndex] : enumerableType;
        }

        /// <summary>
        /// 是否实现了某泛型类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static bool IsKindOfGeneric(this Type type, Type definition) => (FindGenericType(definition, type) != null);

        /// <summary>
        /// 查找泛型类型
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type FindGenericType(this Type definition, Type type)
        {
            while ((type != null) && (type != typeof(object)))
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == definition))
                {
                    return type;
                }

                if (definition.IsInterface)
                {
                    foreach (var type2 in type.GetInterfaces())
                    {
                        var type3 = FindGenericType(definition, type2);

                        if (type3 != null)
                        {
                            return type3;
                        }
                    }
                }

                type = type.BaseType;
            }

            return null;
        }

        /// <summary>
        /// Collection of numeric types.
        /// </summary>
        private static readonly List<Type> NumericTypes = new List<Type>
        {
            typeof(decimal),
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(float), typeof(double)
        };

        /// <summary>
        /// Check if the given type is a numeric type.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns><c>true</c> if it's numeric; otherwise <c>false</c>.</returns>
        public static bool IsNumeric(this Type type)
        {
            return NumericTypes.Contains(type);
        }
    }
}
