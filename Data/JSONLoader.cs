using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using TechTest.Entities;
using TechTest.Interfaces.Data;

namespace TechTest.Data
{
    public class JSONLoader : IJSONLoader
    {
        public Projects LoadFromFile(string filename)
        {
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filename))
            {
                var serializer = new JsonSerializer();         
                return (Projects)serializer.Deserialize(file, typeof(Projects));
            }
        }
    }
}
