namespace UltraBet.Services
{
    using System.IO;
    using System.Xml.Serialization;

    public class Deserializer : IDeserializer
    {
        public T Deserialize<T>(string input, string xmlRootAttribute)
        {
            var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            using var stringReader = new StringReader(input);

            return (T)xmlSerializer.Deserialize(stringReader);
        }

        // private static byte[] SerializeModel<T>(this T m)
        // {
        //    using var ms = new MemoryStream();
        //    new BinaryFormatter().Serialize(ms, m);

        // return ms.ToArray();
        // }

        // private static T DeserializeModel<T>(this byte[] byteArray)
        // {
        //    using var ms = new MemoryStream(byteArray);
        //    return (T)new BinaryFormatter().Deserialize(ms);
        // }
    }
}
