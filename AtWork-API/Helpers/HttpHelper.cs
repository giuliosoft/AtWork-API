using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AtWork_API.Helpers
{
    public static class HttpHelper
    {
        public static T GetFirstHeaderValueOrDefault<T>(this HttpResponseMessage response, string headerKey)
        {
            var toReturn = default(T);
            IEnumerable<string> headerValues;

            if (response.Content.Headers.TryGetValues(headerKey, out headerValues))
            {
                var valueString = headerValues.FirstOrDefault();
                if (valueString != null)
                {
                    return (T)Convert.ChangeType(valueString, typeof(T));
                }
            }

            return toReturn;
        }
        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> keys = null;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }
    }
}