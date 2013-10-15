using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TimeTable.ViewModel.Utils;


namespace TimeTable.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private DefaultUniversityAndGroupManager _defParams;

        public SettingsViewModel()
        {
            getDefaultParameters();
        }

        public void getDefaultParameters()
        {
            DefaultUniversityAndGroupManager defParams  = new DefaultUniversityAndGroupManager();
            _defParams = defParams;
        }
        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string DefaultGroup
        {
            get { return _defParams == null ? string.Empty : _defParams.defaultGroup; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string DefoultUniversity
        {
            get { return _defParams == null ? string.Empty : _defParams.defaultUniversity; }
        }
    }
}
