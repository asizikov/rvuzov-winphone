using System;

namespace TimeTable.Mvvm.Navigation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependsOnViewModelAttribute : Attribute
    {
        public Type ViewModelType { get; private set; }

        public DependsOnViewModelAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}