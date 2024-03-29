﻿using SimaDat.Models.Enums;
using SimaDat.Models.Items;
using System.Collections.Generic;

namespace SimaDat.Models.Characters
{
    public class Hero
    {
        public HeroCalendar Calendar { get; } = new HeroCalendar();

        public int HeroId { get; set; }

        public string Name { get; set; }

        public int CurrentLocationId { get; set; }

        public int Money { get; private set; }

        /// <summary>
        /// How many hours could operate
        /// </summary>
        public int Ttl { get; protected set; }

        public int Strength { get; private set; }

        public int Iq { get; private set; }

        public int Charm { get; private set; }

        public bool HasJumper { get; } = true;

        public IList<Gift> Gifts { get; set; }

        public Hero()
        {
            Gifts = new List<Gift>();
        }

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
                    Iq = ModifyInRange(Iq, improvementPoints, 0, MySettings.MaxIqForHero);
                    break;

                case HeroSkills.Strength:
                    Strength = ModifyInRange(Strength, improvementPoints, 0, MySettings.MaxStrengthForHero);
                    break;

                case HeroSkills.Charm:
                    Charm = ModifyInRange(Charm, improvementPoints, 0, MySettings.MaxCharmForHero);
                    break;
            }
        }

        public void SpendMoney(int money)
        {
            Money -= money;
        }

        protected int ModifyInRange(int initial, int modify, int min, int max)
        {
            int v = initial + modify;

            if (v < min)
            {
                return min;
            }
            if (v > max)
            {
                return max;
            }
            return v;
        }
    }
}
