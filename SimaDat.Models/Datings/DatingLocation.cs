using SimaDat.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Datings
{
    public class DatingLocation
    {
        public bool WasKiss { get; set; } = false;

        public string Name { get; private set; }

        public int Price { get; private set; }

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

        public void IncreaseKissPoints(int points = 1)
        {
            KissPoints += points;
            if (KissPoints < 0)
            {
                KissPoints = 0;
            }
        }
    }
}
