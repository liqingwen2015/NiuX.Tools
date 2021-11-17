// ReSharper disable CheckNamespace

using System.Collections.Generic;

namespace System
{
    public static partial class NiuXStringExtensions
    {
        public static byte? TryParseNullableByte(this string str) => str.IsNullOrEmpty() ? default : str.TryParseByte();

        public static byte? TryParseNullableByte(this string str, byte defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseByte(defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int? TryParseNullableInt(this string str) => str.IsNullOrEmpty() ? default : str.TryParseInt();

        public static int? TryParseNullableInt(this string str, int defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseInt(defaultValue);

        public static long? TryParseNullableLong(this string str) => str.IsNullOrEmpty() ? default : str.TryParseLong();

        public static long? TryParseNullableLong(this string str, long defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseLong(defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? TryParseNullableDecimal(this string str) => str.IsNullOrEmpty() ? default : str.TryParseDecimal();

        public static decimal? TryParseNullableDecimal(this string str, decimal defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseDecimal(defaultValue);

        public static short? TryParseNullableShort(this string str) => str.IsNullOrEmpty() ? default : str.TryParseShort();

        public static short? TryParseNullableShort(this string str, short defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseShort(defaultValue);

        public static float? TryParseNullableFloat(this string str) => str.IsNullOrEmpty() ? default : str.TryParseFloat();

        public static float? TryParseNullableFloat(this string str, float defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseFloat(defaultValue);

        public static bool? TryParseNullableBool(this string str) => str.IsNullOrEmpty() ? default : str.TryParseBool();

        public static bool? TryParseNullableBool(this string str, bool defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseBool(defaultValue);

        public static DateTime? TryParseNullableDateTime(this string str) => str.IsNullOrEmpty() ? default : str.TryParseDateTime();

        public static DateTime? TryParseNullableDateTime(this string str, DateTime defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseDateTime(defaultValue);

        public static Guid? TryParseNullableGuid(this string str) => str.IsNullOrEmpty() ? default : str.TryParseGuid();

        public static Guid? TryParseNullableGuid(this string str, Guid defaultValue) => str.IsNullOrEmpty() ? defaultValue : str.TryParseGuid(defaultValue);

        public static T? TryParseNullable<T>(this string str) where T : struct => str.IsNullOrEmpty() ? default : str.TryParse<T>();

        public static T? TryParseNullable<T>(this string str, T defaultValue) where T : struct => str.IsNullOrEmpty() ? defaultValue : str.TryParse(defaultValue);

    }
}