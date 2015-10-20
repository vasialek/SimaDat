using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Characters
{
    public class Hero
    {
        public int HeroId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Where are am I
        /// </summary>
        public int CurrentLocationId { get; set; }

        /// <summary>
        /// How many hours could operate
        /// </summary>
        public int Ttl { get; private set; }

        public int Strength { get; private set; }

        public int Iq { get; private set; }

        public int Charm { get; private set; }
    }
}
