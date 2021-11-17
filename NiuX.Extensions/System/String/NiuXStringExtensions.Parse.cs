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
        private static readonly Dictionary<Type, Func<string, object>> ToParseMethods = new()
        {
            { typeof(byte), x => x.ParseByte() },
            { typeof(int), x => x.ParseInt() },
            { typeof(long), x => x.ParseLong() },
            { typeof(float), x => x.ParseFloat() },
            { typeof(double), x => x.ParseDouble() },
            { typeof(decimal), x => x.ParseDecimal() },
            { typeof(bool), x => x.ParseBool() },
            { typeof(DateTime), x => x.ParseDateTime() },
            { typeof(Guid), x => x.ParseGuid() },
        };

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte ParseByte(this string str) => byte.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int ParseInt(this string str) => int.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long ParseLong(this string str) => long.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static short ParseShort(this string str) => short.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string str) => DateTime.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static decimal ParseDecimal(this string str) => decimal.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static float ParseFloat(this string str) => float.Parse(str);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static double ParseDouble(this string str) => double.Parse(str);

        public static bool ParseBool(this string str) => bool.Parse(str);

        public static Guid ParseGuid(this string str) => Guid.Parse(str);

        public static T Parse<T>(this string str) where T : struct => (T)ToParseMethods!.GetValue(typeof(T))!(str);

    }
}