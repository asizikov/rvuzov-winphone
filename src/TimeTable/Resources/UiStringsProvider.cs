﻿using TimeTable.ViewModel.Services;

namespace TimeTable.Resources
{
    public class UiStringsProvider : IUiStringsProviders
    {
        public string Auditory
        {
            get { return Strings.Auditory; }
        }

        public string TeachersTimeTable
        {
            get { return Strings.TeachersTimeTable; }
        }

        public string Group
        {
            get { return Strings.Group; }
        }

        public string Settings
        {
            get { return Strings.Settings; }
        }

        public string AddToFavorited
        {
            get { return Strings.ToFavorites; }
        }

        public string Favorites
        {
            get { return Strings.Favorite; }
        }
    }
}