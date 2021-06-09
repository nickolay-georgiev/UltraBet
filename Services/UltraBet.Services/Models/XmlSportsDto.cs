namespace UltraBet.Services.Models
{
    using System.Xml.Serialization;

    [XmlType("XmlSports")]
    public class XmlSportsDto
    {
        [XmlElement("Sport")]
        public SportDto SportDto { get; set; }
    }
}
