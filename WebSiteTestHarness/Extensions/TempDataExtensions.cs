using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace TechTest.WebSiteTestHarness.Extensions
{
    /// <summary>
    /// Extension class for TempData to allow serialization and 
    /// deserailization of complex objects
    /// </summary>
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            if (value == null)
            {
                tempData[key] = null;
                tempData.Remove(key);
            }
            else
            {
                var serValue = JsonConvert.SerializeObject(value);
                tempData[key] = serValue;
            }
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;

            if (tempData.TryGetValue(key, out o))
            {
                tempData.Remove(key);
            }

            var settings = new JsonSerializerSettings();
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
