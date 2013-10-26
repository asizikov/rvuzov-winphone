using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Networking
{
    public class Deserializer
    {
        [CanBeNull, Pure]
        public T Deserialize<T>([CanBeNull] string json) where T : new()
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}