namespace UltraBet.Data.Models
{
    using System.Collections.Generic;

    using UltraBet.Data.Common.Models;

    public class EventCategory : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
