using SimaDat.Models.Characters;

namespace SimaDat.Models.Datings
{
    public class DatingLocation
    {
        public bool WasKiss { get; set; } = false;

        public string Name { get; }

        public int Price { get; }

        public int DatingLocationId { get; set; }

        public Hero Hero { get; set; } = null;

        public Girl Girl { get; set; } = null;

        public int KissPoints { get; set; } = 0;

        public bool IsOver { get; set; }

        public DatingLocation(string name, int price)
        {
            Name = name;
            Price = price;
            IsOver = false;
        }
    }
}
