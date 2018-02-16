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
        public string Name { get; private set; }

        public int Price { get; private set; }

        public int DatingLocationId { get; set; }

        public Hero Hero { get; set; } = null;

        public Girl Girl { get; set; } = null;

        public int KissPoints { get; private set; } = 0;

        public DatingLocation(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
