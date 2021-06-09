namespace UltraBet.Services.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Match")]
    public class MatchDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute]
        public DateTime StartDate { get; set; }

        [XmlAttribute]
        public string MatchType { get; set; }

        [XmlElement("Bet")]
        public BetDto[] Bets { get; set; }
    }
}
