using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utility.Extensions
{
    public static class UriExtensions
    {
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);

            NameValueCollection query = ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = QueryToString(query);

            return uriBuilder.Uri;
        }

        public static NameValueCollection GetParameters(this Uri url)
        {
            int idx = url.Query.IndexOf('?');
            string queryString = idx < url.Query.Length - 1 ? url.Query.Substring(idx + 1) : string.Empty;

            return ParseQueryString(queryString);
        }

        private static string QueryToString(NameValueCollection parameters)
        {
            List<string> paramsStr = new List<string>();
            foreach (string key in parameters.AllKeys)
            {
                paramsStr.Add($"{key}={parameters.Get(key)}");
            }

            return string.Join("&", paramsStr);
        }

        private static NameValueCollection ParseQueryString(string queryString)
        {
            NameValueCollection result = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                string[] paramsStr = queryString.Split('&');
                foreach (string paramStr in paramsStr)
                {
                    string[] paramArr = paramStr.Split('=');
                    if (paramArr.Length != 2)
                        continue;

                    string key = paramArr[0].Trim();
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    string value = paramArr[1].Trim();

                    result.Add(key, value);
                }
            }

            return result;
        }
    }
}
