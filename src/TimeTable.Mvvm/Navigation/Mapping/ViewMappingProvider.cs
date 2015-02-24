using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation.Mapping
{
    public class ViewMappingProvider : IViewMappingProvider
    {
        private IViewModelNameExtractor ViewModelNameExtractor { get; set; }
        private IViewNameBuilder ViewNameBuilder { get; set; }

        [PublicAPI]
        public ViewMappingProvider()
            : this(new ViewModelNameExtractor(), new ViewNameBuilder())
        {
        }

        [PublicAPI]
        public ViewMappingProvider([NotNull] IViewModelNameExtractor viewModelNameExtractor,
            [NotNull] IViewNameBuilder viewNameBuilder)
        {
            if (viewModelNameExtractor == null) throw new ArgumentNullException("viewModelNameExtractor");
            if (viewNameBuilder == null) throw new ArgumentNullException("viewNameBuilder");

            ViewModelNameExtractor = viewModelNameExtractor;
            ViewNameBuilder = viewNameBuilder;
        }

        public IEnumerable<string> GetPossibleMappings(Type viewModel)
        {
            var viewModelName = ViewModelNameExtractor.Extract(viewModel);
            var possibleViewNames = ViewNameBuilder.Build(viewModelName);
            return possibleViewNames;
        }
    }
}