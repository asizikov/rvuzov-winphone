using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeTable.Mvvm.Navigation.Mapping
{
    internal class ViewModelNameExtractor : IViewModelNameExtractor
    {
        private ICollection<string> ViewModelSuffixes { get; set; }

        public ViewModelNameExtractor()
        {
            ViewModelSuffixes = new HashSet<string>(new[] { "ViewModel", "VM" });
        }

        public string Extract(Type viewModelType)
        {
            if (viewModelType == null) throw new ArgumentNullException("viewModelType");

            var name = viewModelType.Name;
            return
                ViewModelSuffixes.Where(name.EndsWith)
                    .Select(viewModelSuffix => name.Substring(0, name.Length - viewModelSuffix.Length))
                    .FirstOrDefault();
        }
    }
}