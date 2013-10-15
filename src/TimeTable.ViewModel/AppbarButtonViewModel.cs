using System.Windows.Input;
using JetBrains.Annotations;

namespace TimeTable.ViewModel
{
    public class AppbarButtonViewModel
    {
        [UsedImplicitly]
        public string Text { get; set; }
        [UsedImplicitly]
        public ICommand Command { get; set; }

        public string IconUri { get; set; }
        
    }
}
