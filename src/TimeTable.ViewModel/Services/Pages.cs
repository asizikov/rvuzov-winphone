using JetBrains.Annotations;

namespace TimeTable.ViewModel.Services
{
    public static class Pages
    {
        [NotNull]
        public static readonly string Groups = "/View/GroupsPage.xaml";

        [NotNull]
        public static readonly string Universities = "/View/TmpPage.xaml";

        [NotNull] public static readonly string Lessons = "/View/LessonsPage.xaml";
    }
}
