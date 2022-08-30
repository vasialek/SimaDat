using SimaDat.Models.Enums;

namespace SimaDat.Models.Characters
{
    public class Appearance
    {
        public Hairs Hair { get; set; }

        public int Height { get; set; }

        public int Bust { get; set; }

        public int Waist { get; set; }

        public int Hipp { get; set; }

        public Appearance(int height, int bust, int waist, int hipp)
        {
            Height = height;
            Bust = bust;
            Waist = waist;
            Hipp = hipp;
        }

        public override string ToString()
        {
            return $"{Height} cm. {Bust}-{Waist}-{Hipp}. {Hair}";
        }
    }
}
