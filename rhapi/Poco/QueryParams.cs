using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace rhapi.Poco
{
    public class QueryParams
    {
        public virtual string GetParamString()
        {
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryParams = new List<string>();

            foreach (var prop in props)
            {
                var value = prop.GetValue(this);
                if (value is null) continue;

                var fromQuery = prop.GetCustomAttribute<FromQueryAttribute>();
                var jsonProperty = prop.GetCustomAttribute<JsonPropertyAttribute>();
                var name = fromQuery?.Name ?? jsonProperty?.PropertyName ?? prop.Name.ToSnakeCase();

                var stringValue = value switch
                {
                    DateTime dt => dt.ToIso8601(),
                    Guid g      => g.ToString(),
                    Enum e      => e.ToString(),
                    decimal m   => m.ToString(CultureInfo.InvariantCulture),
                    _           => null
                };
                if (stringValue is null) continue;

                queryParams.Add($"{name}={stringValue}");
            }

            queryParams.AddRange(GetCustomParams());

            return queryParams.Any() ? $"?{string.Join('&', queryParams)}" : string.Empty;
        }

        protected virtual IEnumerable<string> GetCustomParams() => [];
    }
}
