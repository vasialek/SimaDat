using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Characters
{
    public class Appearance
    {
        public Hairs Hair { get; set; }

        public int Height { get; set; }

        public int Bust { get; set; }

        public int Waist { get; set; }

        public int Hipp { get; set; }

        public Appearance()
        {
        }

        public Appearance(int height, int bust, int waist, int hipp)
        {
            Height = height;
            Bust = bust;
            Waist = waist;
            Hipp = hipp;
        }

        public override string ToString()
        {
            return String.Format("{0} cm. {1}-{2}-{3}. {4}", Height, Bust, Waist, Hipp, Hair);
        }
    }
}
