using System;
using System.Collections.Generic;
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
            throw new Exception("Can't restore context");
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
            throw new Exception("Can't restore context");
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes, 0, base64EncodedBytes.Length);
        }
    }
}
