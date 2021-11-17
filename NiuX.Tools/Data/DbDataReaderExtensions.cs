using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NiuX.Data
{
    public static class DbDataReaderExtensions
    {
        public static List<T> ReadAsList<T>(this DbDataReader reader) where T : new() => Inner.ReadAsList<T>(reader);

        public static List<T> ReadAsCompatibleList<T>(this DbDataReader reader) where T : new() => Inner.ReadAsCompatibleList<T>(reader);

        private static class Inner
        {
            private static readonly ParameterExpression ParameterExpression;

            private static readonly Type DbDataReaderType;

            static Inner()
            {
                DbDataReaderType = typeof(DbDataReader);
                ParameterExpression = Expression.Parameter(DbDataReaderType);
            }

            internal static List<T> ReadAsList<T>(DbDataReader reader) where T : new() => InnerCore<T>.Main.ReadAsList(reader);

            internal static List<T> ReadAsCompatibleList<T>(DbDataReader reader) where T : new() => InnerCore<T>.Compatibility.InnerReadAsList(reader);

            private static class InnerCore<T> where T : new()
            {
                private static readonly ConcurrentDictionary<string, Func<DbDataReader, List<T>>> Readers = new();

                private static readonly ParameterExpression _listVariable;

                private static readonly LabelTarget _listLabelTarget;

                private static readonly Type _listType;

                private static readonly Type _modelType;

                static InnerCore()
                {
                    _listType = typeof(List<T>);
                    _modelType = typeof(T);
                    _listLabelTarget = Expression.Label(_listType);
                    _listVariable = Expression.Variable(_listType);
                }

                /// <summary>
                /// 主
                /// </summary>
                internal static class Main
                {
                    internal static List<T> ReadAsList(DbDataReader dataRecord) =>
                        InnerCore<T>.ReadAsList(dataRecord, x => Expression.Call(Expression.Constant(dataRecord, DbDataReaderType), GetMethodOfGetReaderValue(x.Value)!, Expression.Constant(x.Key, typeof(int))));
                }

                /// <summary>
                /// 兼容
                /// </summary>
                internal static class Compatibility
                {
                    internal static List<T> InnerReadAsList(DbDataReader dataRecord) =>
                        ReadAsList(dataRecord, x => Expression.Call(null, typeof(Convert).GetMethod("To" + GetTypeAlias(x.Value), new[] { typeof(object) })!,
                            Expression.Call(Expression.Constant(dataRecord, DbDataReaderType), DbDataReaderType.GetMethod("GetValue", new[] { typeof(int) })!,
                                Expression.Constant(0, typeof(int)))));
                }


                private static List<T> ReadAsList(DbDataReader reader, Func<KeyValuePair<int, PropertyInfo>, Expression> func) =>
                    Readers.GetOrAdd(string.Concat(Enumerable.Range(0, reader.FieldCount).Select(reader.GetName)),
                        _ => Expression.Lambda<Func<DbDataReader, List<T>>>(
                         Expression.Block(new List<ParameterExpression>() { _listVariable },
                             Expression.Assign(_listVariable, Expression.New(_listType)),
                             Expression.Loop(Expression.IfThenElse(Expression.Equal(
                                 Expression.Call(ParameterExpression, DbDataReaderType.GetMethod("Read")!), Expression.Constant(true)),
                                 Expression.Call(_listVariable, _listType.GetMethod("Add", new[] { _modelType })!, CreateExpressionOfMemberInit(reader, func)),
                                 Expression.Break(_listLabelTarget, _listVariable))),
                             Expression.Label(_listLabelTarget, _listVariable)), ParameterExpression).Compile())(reader);


                private static Expression CreateExpressionOfMemberInit(DbDataReader reader, Func<KeyValuePair<int, PropertyInfo>, Expression> func) =>
                    Expression.MemberInit(Expression.New(_modelType), GetMapping(reader).Select(p =>
                        Expression.Bind(p.Value, Expression.Condition(
                            Expression.Call(Expression.Constant(reader, DbDataReaderType), DbDataReaderType.GetMethod("IsDBNull", new[] { typeof(int) })!,
                                Expression.Constant(p.Key, typeof(int))), CreateConstExpression(p.Value), Expression.Convert(func(p), p.Value.PropertyType)))));

                private static MethodInfo? GetMethodOfGetReaderValue(string dataTypeAlias) => DbDataReaderType.GetMethod("Get" + dataTypeAlias, new[] { typeof(int) });



                private static MethodInfo? GetMethodOfGetReaderValue(PropertyInfo property) => GetMethodOfGetReaderValue(GetTypeAlias(property));

                private static Expression CreateConstExpression(PropertyInfo property) =>
                    Expression.Constant(property.PropertyType == typeof(string) || property.PropertyType.IsNullable() ? null : property.PropertyType.GetDefaultValue(), property.PropertyType);

                private static Dictionary<int, PropertyInfo> GetMapping(DbDataReader reader) =>
                    Enumerable.Range(0, reader.FieldCount).ToDictionary(x => x, reader.GetName)
                        .ToDictionary(x => x.Key, p => Array.Find(_modelType.GetProperties(), x => p.Value == x.GetCustomAttribute<ColumnAttribute>()?.Name || p.Value == x.Name || p.Value == x.Name.ToSnakeCase()))
                        .Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        /// <summary>
        /// 类型别名
        /// </summary>
        private static readonly Dictionary<Type, string> TypeAlias = new()
        {
            { typeof(int?), "Int32" },
            { typeof(int), "Int32" },
            { typeof(byte?), "Int16" },
            { typeof(byte), "Int16" },
            { typeof(long?), "Int64" },
            { typeof(long), "Int64" },
            { typeof(decimal), "Decimal" },
            { typeof(string), "String" },
            { typeof(DateTime?), "DateTime" },
            { typeof(DateTime), "DateTime" },
        };

        private static string GetTypeAlias(PropertyInfo property) => TypeAlias.First(x => x.Key == property.PropertyType).Value;
    }
}