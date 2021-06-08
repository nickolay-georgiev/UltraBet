namespace UltraBet.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Sport")]
    public class SportDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlElement("Event")]
        public EventDto[] Events { get; set; }
    }
}
