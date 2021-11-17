// ReSharper disable CheckNamespace

using System.Collections.Generic;

namespace System
{
    public static partial class NiuXStringExtensions
    {
        #region private fields

        /// <summary>
        /// 待转换方法
        /// </summary>
        private static readonly Dictionary<Type, Func<string, object>> ToTryParseMethods = new()
        {
            { typeof(byte), x => x.TryParseByte() },
            { typeof(int), x => x.TryParseInt() },
            { typeof(long), x => x.TryParseLong() },
            { typeof(float), x => x.TryParseFloat() },
            { typeof(double), x => x.TryParseDouble() },
            { typeof(decimal), x => x.TryParseDecimal() },
            { typeof(bool), x => x.TryParseBool() },
            { typeof(DateTime), x => x.TryParseDateTime() },
            { typeof(Guid), x => x.TryParseGuid() },
        };

        /// <summary>
        /// 待转换含默认值的方法
        /// </summary>
        private static readonly Dictionary<Type, Func<string, object, object>> ToTryParseMethodsOfContainDefaultValue = new()
        {
            { typeof(byte), (x, y) => x.TryParseByte((byte)y) },
            { typeof(int), (x, y) => x.TryParseInt((int)y) },
            { typeof(long), (x, y) => x.TryParseLong((long)y) },
            { typeof(float), (x, y) => x.TryParseFloat((float)y) },
            { typeof(double), (x, y) => x.TryParseDouble((double)y) },
            { typeof(decimal), (x, y) => x.TryParseDecimal((decimal)y) },
            { typeof(bool), (x, y) => x.TryParseBool((bool)y) },
            { typeof(DateTime), (x, y) => x.TryParseDateTime((DateTime)y) },
            { typeof(Guid), (x, y) => x.TryParseGuid((Guid)y) },
        };

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte TryParseByte(this string str, byte defaultValue = default)
            => byte.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int TryParseInt(this string str, int defaultValue = default)
            => int.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long TryParseLong(this string str, long defaultValue = default)
            => long.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static short TryParseShort(this string str, short defaultValue = default)
            => short.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DateTime TryParseDateTime(this string str, DateTime defaultValue = default)
            => DateTime.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static decimal TryParseDecimal(this string str, decimal defaultValue = default)
            => decimal.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static float TryParseFloat(this string str, float defaultValue = default) => float.TryParse(str, out var result) ? result : defaultValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double TryParseDouble(this string str, double defaultValue = default) => double.TryParse(str, out var result) ? result : defaultValue;

        public static bool TryParseBool(this string str, bool defaultValue = default) => bool.TryParse(str, out var result) ? result : defaultValue;

        public static Guid TryParseGuid(this string str, Guid defaultValue = default) => Guid.TryParse(str, out var result) ? result : defaultValue;

        public static T TryParse<T>(this string str) where T : struct => ToTryParseMethods.TryGetValue(typeof(T), out var func) ? (T)func(str) : default;

        public static T TryParse<T>(this string str, T defaultValue) where T : struct => ToTryParseMethodsOfContainDefaultValue.TryGetValue(typeof(T), out var func) ? (T)func(str, defaultValue) : defaultValue;

    }
}