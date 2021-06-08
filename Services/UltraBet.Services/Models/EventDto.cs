namespace UltraBet.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Event")]
    public class EventDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute]
        public bool IsLive { get; set; }

        [XmlAttribute("CategoryID")]
        public string CategoryId { get; set; }

        [XmlElement("Match")]
        public MatchDto[] Matches { get; set; }
    }
}
