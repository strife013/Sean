using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sean.Utilities
{
    public static class BinaryClone
    {
        public static T From<T>(T source) where T : class
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, source);
            ms.Flush();
            ms.Position = 0;
            return ((T)bf.Deserialize(ms));
        }
    }
}
