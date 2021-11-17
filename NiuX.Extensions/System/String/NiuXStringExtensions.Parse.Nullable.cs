// ReSharper disable CheckNamespace

using System.Collections.Generic;

namespace System
{
    public static partial class NiuXStringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte? ParseNullableByte(this string str) => str.IsNullOrEmpty() ? default : str.ParseByte();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int? ParseNullableInt(this string str) => str.IsNullOrEmpty() ? default : str.ParseInt();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long? ParseNullableLong(this string str) => str.IsNullOrEmpty() ? default : str.ParseLong();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ParseNullableDecimal(this string str) => str.IsNullOrEmpty() ? default : str.ParseDecimal();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short? ParseNullableShort(this string str) => str.IsNullOrEmpty() ? default : str.ParseShort();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float? ParseNullableFloat(this string str) => str.IsNullOrEmpty() ? default : str.ParseFloat();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ParseNullableBool(this string str) => str.IsNullOrEmpty() ? default : str.ParseBool();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ParseNullableDateTime(this string str) => str.IsNullOrEmpty() ? default : str.ParseDateTime();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid? ParseNullableGuid(this string str) => str.IsNullOrEmpty() ? default : str.ParseGuid();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T? ParseNullable<T>(this string str) where T : struct => str.IsNullOrEmpty() ? default : str.Parse<T>();
    }
}