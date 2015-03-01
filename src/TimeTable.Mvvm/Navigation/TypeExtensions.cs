using System;

namespace TimeTable.Mvvm.Navigation
{
    public static class TypeExtensions
    {
        public static Uri GetUri(this Type viewType)
        {
            var assembly = viewType.Assembly;
            var name = assembly.GetName().Name;
            var uri = viewType.FullName.Replace(name, string.Empty).Replace(".", "/");
            return new Uri(string.Format("/{0};component{1}.xaml", name, uri), UriKind.Relative);
        }
    }
}