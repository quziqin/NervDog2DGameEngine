using System.IO;
using System.Xml.Serialization;

namespace NervDog.Utilities
{
    public static class XmlHelper
    {
        //Export serializable object
        public static void ExportToFile<T>(T obj, string filename) where T : class
        {
            var xs = new XmlSerializer(typeof (T));
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(fs, obj);
            }
        }

        public static T LoadFromFile<T>(string filename) where T : class
        {
            var xs = new XmlSerializer(typeof (T));
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var obj = xs.Deserialize(fs) as T;
                return obj;
            }
        }
    }
}