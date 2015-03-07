using System;
using System.ComponentModel;
using System.Windows.Input;
using JetBrains.Annotations;

namespace TimeTable.ViewModel.OrganizationalStructure
{
    public interface ISearchViewModel
    {
        string Query { get; set; }
        Action<bool> OnLock { get; set; }
        ICommand ShowSearchBoxCommand { get; }
        ICommand OnFoundCommand { get; }

        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        bool IsSearchBoxVisible { get; set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        bool IsLoading { get; }

        void ResetSearchState();
        event PropertyChangedEventHandler PropertyChanged;
    }
}