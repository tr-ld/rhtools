using System;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace rhapi
{
    public static class Extensions
    {
        internal static string AsPythonJson(this JObject toSerialize)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var jw = new JsonTextWriter(sw))
            {
                jw.QuoteChar = '\'';
                toSerialize.WriteTo(jw);
            }

            var bodyJson = sb.ToString().Replace("':", "': ").Replace("','", "', '");
            return bodyJson;
        }

        private static readonly Regex _snakeCaseRegex = new(@"([a-z0-9])([A-Z])", RegexOptions.Compiled);
        internal static string ToSnakeCase(this string toConvert) { return _snakeCaseRegex.Replace(toConvert, "$1_$2").ToLowerInvariant(); }

        private const string ISO_8601 = "yyyy-MM-ddTHH:mm:ssZ";
        internal static string ToIso8601(this DateTime dt) => dt.ToUniversalTime().ToString(ISO_8601);
    }
}
