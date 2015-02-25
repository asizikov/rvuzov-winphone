using System;
using System.Collections.Generic;
using System.Linq;
using TimeTable.Mvvm.Navigation.Serialization;

namespace TimeTable.Mvvm.Navigation
{
    public static class NavigationQueryExtension
    {
        private static string Key = "NavigationContext";

        public static NavigationContext RestoreContext(this IDictionary<string, string> query)
        {
            string encodedContext;
            if (query.TryGetValue(Key, out encodedContext))
            {
                var json = Base64Decode(encodedContext);
                var navigationSerializer = new NavigationSerializer();
                return navigationSerializer.Deserialize<NavigationContext>(json);
            }
            var actualQuery = query.Select(s => string.Format(" [{0}:{1}]", s.Key, s.Value)).Aggregate((s, a) => s + a);
            throw new NavigationException("Can't restore context, actual query is:" + actualQuery);
        }

        public static NavigationContext<TData> RestoreContext<TData>(this IDictionary<string, string> query)
        {
            string encodedContext;
            if (query.TryGetValue(Key, out encodedContext))
            {
                var json = Base64Decode(encodedContext);
                var navigationSerializer = new NavigationSerializer();
                return navigationSerializer.Deserialize<NavigationContext<TData>>(json);
            }
            var actualQuery = query.Select(s =>string.Format(" [{0}:{1}]",s.Key, s.Value) ).Aggregate((s, a) => s + a);
            throw new NavigationException("Can't restore context, actual query is:" + actualQuery);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes, 0, base64EncodedBytes.Length);
        }
    }
}
