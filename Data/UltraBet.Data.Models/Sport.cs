namespace UltraBet.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UltraBet.Data.Common.Models;

    public class Sport : BaseDeletableModel<string>
    {
        public Sport()
        {
            this.Events = new HashSet<Event>();
        }

        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
