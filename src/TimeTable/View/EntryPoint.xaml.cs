using System;
using System.Windows.Navigation;

namespace TimeTable.View
{
    public partial class EntryPoint
    {
        public EntryPoint()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            throw new InvalidOperationException("We should never havigate here!");
        }
    }
}