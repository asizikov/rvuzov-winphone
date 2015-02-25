using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.ViewModel.ApplicationLevel
{
    public sealed class DefaultViewModel
    {
        private readonly Me _model;
        private readonly INavigationService _navigationService;

        public DefaultViewModel(Me model, INavigationService navigationService)
        {
            _model = model;
            _navigationService = navigationService;
            ChangeDefacultCommand = new SimpleCommand(ChangeDefault);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Name
        {
            get { return _model.Teacher != null ? _model.Teacher.Name : _model.DefaultGroup.GroupName; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string University
        {
            get { return _model.University.Name; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ChangeDefacultCommand { get; private set; }

        private void ChangeDefault()
        {
            _navigationService.NavigateTo<UniversitiesPageViewModel, Reason>(Reason.ChangeDefault);
        }
    }
}