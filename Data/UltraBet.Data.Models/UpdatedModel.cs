namespace UltraBet.Data.Models
{

    using UltraBet.Data.Common.Models;

    public class UpdatedModel : BaseModel<int>
    {
        public string Name { get; set; }

        public byte[] Model { get; set; }
    }
}
