namespace UltraBet.Services
{
    using System.IO;
    using System.Xml.Serialization;

    public class SerializationService : ISerializationService
    {
        private const string XmlRootAttribute = "XmlSports";

        public T DeserializeSportData<T>(string input)
        {
            var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(XmlRootAttribute));

            using var stringReader = new StringReader(input);

            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
