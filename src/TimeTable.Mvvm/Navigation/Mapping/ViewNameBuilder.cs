using System.Collections.Generic;
using System.Linq;

namespace TimeTable.Mvvm.Navigation.Mapping
{
    internal class ViewNameBuilder : IViewNameBuilder
    {
        private ICollection<string> ViewSuffixes { get; set; }

        public ViewNameBuilder()
        {
            ViewSuffixes = new HashSet<string>(new[] { "Page", "View" });
        }

        public IEnumerable<string> Build(string viewModelName)
        {
            return ViewSuffixes.Select(view => string.Concat(viewModelName, (string) view));
        }
    }
}