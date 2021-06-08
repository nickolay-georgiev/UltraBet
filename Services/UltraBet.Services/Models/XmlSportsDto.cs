namespace UltraBet.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("XmlSports")]
    public class XmlSportsDto
    {
        [XmlElement("Sport")]
        public SportDto SportDto { get; set; }
    }
}
