
using Newtonsoft.Json;

namespace NiuX
{
    public static class JsonExtensions
    {
        public static string ToJsonString(this object obj) => JsonConvert.SerializeObject(obj);

        public static T? ToJson<T>(this string str) => JsonConvert.DeserializeObject<T>(str);
    }
}