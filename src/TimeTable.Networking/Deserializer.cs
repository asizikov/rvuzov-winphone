using Newtonsoft.Json;

namespace TimeTable.Networking
{
    public class Deserializer
    {
        public Deserializer()
        {

        }

           public T Deserialize<T>(string json)
           {
               var result = JsonConvert.DeserializeObject<T>(json);
               return result;
           }
    }
}