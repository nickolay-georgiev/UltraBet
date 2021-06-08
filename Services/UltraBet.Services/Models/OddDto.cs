namespace UltraBet.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Odd")]
    public class OddDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute]
        public double Value { get; set; }

        [XmlAttribute]
        public string SpecialBetValue { get; set; }
    }
}
