using System.Globalization;

namespace TimeTable.ViewModel.Utils
{
    public static class StringExtentions
    {
        private static readonly CultureInfo InvariantCulture;

        static StringExtentions()
        {
            InvariantCulture = CultureInfo.InvariantCulture;
        }

        public static bool IgnoreCaseContains(this string text, string search)
        {
            return InvariantCulture.CompareInfo.IndexOf(text, search, CompareOptions.IgnoreCase) >= 0;
        }
    }
}