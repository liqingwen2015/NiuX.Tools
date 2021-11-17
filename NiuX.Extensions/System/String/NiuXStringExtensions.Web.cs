
// ReSharper disable CheckNamespace

namespace System.Web
{
    public static partial class NiuXStringExtensions
    {
        public static string ToHtmlDecode(this string str) => HttpUtility.HtmlDecode(str);

        public static string ToHtmlEncode(this string str) => HttpUtility.HtmlEncode(str);
    }
}