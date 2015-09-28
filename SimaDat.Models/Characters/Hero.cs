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
    }
}
