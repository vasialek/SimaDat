using SimaDat.Models.Enums;

namespace SimaDat.Models.Items
{
    public class Gift
    {
        public int GiftId { get; set; }

        public GiftTypes GiftTypeId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int FirendshipPoints { get; set; }
    }
}
