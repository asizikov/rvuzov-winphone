using System;
using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation.Mapping
{
    public interface IViewModelNameExtractor
    {
        [CanBeNull]
        string Extract([NotNull] Type viewModelType);
    }
}