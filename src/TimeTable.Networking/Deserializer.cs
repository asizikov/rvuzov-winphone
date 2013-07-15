using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Networking
{
    public class Deserializer
    {
        [CanBeNull]
        public T Deserialize<T>([CanBeNull] string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}