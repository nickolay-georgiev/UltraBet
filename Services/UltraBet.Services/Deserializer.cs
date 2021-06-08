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
    }
}
