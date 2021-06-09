namespace UltraBet.Services.Models
{
    using System.Xml.Serialization;

    [XmlType("Bet")]
    public class BetDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute]
        public bool IsLive { get; set; }

        [XmlElement("Odd")]
        public OddDto[] Odds { get; set; }
    }
}
