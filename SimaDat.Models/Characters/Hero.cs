using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Enums;

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
        public int Ttl { get; protected set; }

        public int Strength { get; private set; }

        public int Iq { get; private set; }

        public int Charm { get; private set; }

        /// <summary>
        /// Resets TTL to max
        /// </summary>
        public void ResetTtl()
        {
            Ttl = MySettings.MaxTtlForHero;
        }

        public void UseTtl(int ttlToUse)
        {
            Ttl -= ttlToUse;
        }

        public void ModifySkill(HeroSkills skill, int improvementPoints)
        {
            switch (skill)
            {
                case HeroSkills.None:
                    break;
                case HeroSkills.Iq:
                    Iq += improvementPoints;
                    break;
                case HeroSkills.Strength:
                    Strength += improvementPoints;
                    break;
                case HeroSkills.Charm:
                    Charm += improvementPoints;
                    break;
                default:
                    break;
            }
        }
    }
}
