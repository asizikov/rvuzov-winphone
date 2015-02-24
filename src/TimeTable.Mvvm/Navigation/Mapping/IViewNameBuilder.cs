using System.Collections.Generic;

namespace TimeTable.Mvvm.Navigation.Mapping
{
    public interface IViewNameBuilder
    {
        IEnumerable<string> Build(string viewModelName);
    }
}