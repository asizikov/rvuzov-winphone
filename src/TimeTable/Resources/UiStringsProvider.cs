using TimeTable.ViewModel.Services;

namespace TimeTable.Resources
{
    public class UiStringsProvider : IUiStringsProviders
    {
        public string Auditory
        {
            get
            {
                return Strings.Auditory;
            }
        }

        public string TeachersTimeTable
        {
            get
            {
                return Strings.TeachersTimeTable;
            }
        }
    }
}
