using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Model;
using TimeTable.Model.Internal;

namespace TimeTable.ViewModel.Data
{
    public class UniversitiesCache
    {
        private const int VERSION = 1;
        private Dictionary<int, UniversityItem> _cache = new Dictionary<int, UniversityItem>();

        private DataWriter _dataWriter = new DataWriter();

        public void AddUniversity(University university)
        {
            if (!_cache.ContainsKey(university.Id))
            {
                _cache.Add(university.Id, new UniversityItem
                {
                    Id = university.Id,
                    Data = university,
                    Faculties = new List<FacultyItem>()
                });
            }
        }

        public void AddFaculty(int universityId, Faculty faculty)
        {
            if (_cache.ContainsKey(universityId))
            {
                if (_cache[universityId].Faculties.Any(f => f.Id == faculty.Id)) return;

                _cache[universityId].Faculties.Add(new FacultyItem
                {
                    Id = faculty.Id,
                    Data = faculty,
                    Groups = new List<Group>()
                });
            }
        }

        public void AddGroup(int universityId, int facultyId, Group group)
        {
            if (_cache.ContainsKey(universityId))
            {
                if (_cache[universityId].Faculties.All(f => f.Id != facultyId)) return;

                var faculty = _cache[universityId].Faculties.First(f => f.Id == facultyId);

                if (faculty.Groups.Any(g => g.Id == group.Id)) return;

                faculty.Groups.Add(group);
            }
        }

        [CanBeNull]
        public Faculty GetFacultyByGroupAndUniversityIds(int universityId, int groupId)
        {
            if (_cache.ContainsKey(universityId))
            {
                var university = _cache[universityId];
                foreach (var faculty in university.Faculties)
                {
                    if (faculty.Groups.Any(g => g.Id == groupId)) return faculty.Data;
                }
                return null;
            }
            return null;
        }

        public void Save()
        {
            var storage = WrapData();
            _dataWriter.Save(storage);
        }

        private Storage WrapData()
        {
            var storage = new Storage
            {
                Version = VERSION, 
                Data = _cache.Select(u => u.Value).ToList()
            };
            return storage;
        }

        public void Load()
        {
            var storage = _dataWriter.LoadStorage();
            _cache = storage.Data.ToDictionary(ui => ui.Id);
        }


        private static string SerializeToStrng(Storage favs)
        {
            return JsonConvert.SerializeObject(favs);
        }
    }
}