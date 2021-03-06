﻿using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Domain.Internal;
using TimeTable.Domain.OrganizationalStructure;

namespace TimeTable.ViewModel.FavoritedTimeTables
{
    public class FavoritedItemsManager
    {
        [NotNull] private const string FAVORITES = "Favorites";
        [NotNull] private readonly Favorites _favoritedItems;

        public FavoritedItemsManager()
        {
            _favoritedItems = LoadFavorites();
        }

        [NotNull, Pure]
        private static Favorites LoadFavorites()
        {
            Favorites favs;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(FAVORITES))
            {
                favs = GetEmptyCollection();
                IsolatedStorageSettings.ApplicationSettings.Add(FAVORITES, SerializeToStrng(favs));
            }
            else
            {
                var favsJsonString = (string) IsolatedStorageSettings.ApplicationSettings[FAVORITES];
                favs = DeserializeFromString(favsJsonString);
            }
            return favs;
        }

        [NotNull, Pure]
        private static Favorites DeserializeFromString(string favsJsonString)
        {
            var deserializedFavs = JsonConvert.DeserializeObject<Favorites>(favsJsonString);
            return deserializedFavs ?? GetEmptyCollection();
        }

        private static string SerializeToStrng(Favorites favs)
        {
            return JsonConvert.SerializeObject(favs);
        }

        [NotNull, Pure]
        private static Favorites GetEmptyCollection()
        {
            var favs = new Favorites
            {
                Items = new List<FavoritedItem>()
            };
            return favs;
        }

        [NotNull, Pure]
        public IEnumerable<FavoritedItem> GetFavorites()
        {
            return new List<FavoritedItem>(_favoritedItems.Items);
        }

        public void Add(bool isTeacher, int id, string title, University university, int facultyId)
        {
            var newItem = new FavoritedItem
            {
                Id = id,
                Type = isTeacher ? FavoritedItemType.Teacher : FavoritedItemType.Group,
                Title = title,
                University = university,
                Faculty = new Faculty
                {
                    Id = facultyId
                }
            };
            _favoritedItems.Items.Add(newItem);
        }

        public void Remove(bool isTeacher, int id, University university, int facultyId)
        {
            if (isTeacher)
            {
                var teacher = _favoritedItems.Items.FirstOrDefault(
                    i => i.Type == FavoritedItemType.Teacher && i.University.Id == university.Id && id == i.Id);
                if (teacher != null)
                {
                    _favoritedItems.Items.Remove(teacher);
                }
            }
            else
            {
                var group = _favoritedItems.Items.FirstOrDefault(
                    i => i.Type == FavoritedItemType.Group && i.Faculty.Id == facultyId && id == i.Id);
                if (group != null)
                {
                    _favoritedItems.Items.Remove(group);
                }
            }
        }

        [Pure]
        public bool IsGroupFavorited(int facultyId, int groupId)
        {
            return _favoritedItems.Items
                                  .Where(item => item.Type == FavoritedItemType.Group)
                                  .Any(itm => itm.Faculty.Id == facultyId && itm.Id == groupId);
        }

        [Pure]
        public bool IsTeacherFavorited(int universityId, int groupId)
        {
            return _favoritedItems.Items
                                  .Where(item => item.Type == FavoritedItemType.Teacher)
                                  .Any(itm => itm.University.Id == universityId && itm.Id == groupId);
        }

        public void Save()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(FAVORITES))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(FAVORITES, SerializeToStrng(_favoritedItems));
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[FAVORITES] = SerializeToStrng(_favoritedItems);
            }
        }
    }
}