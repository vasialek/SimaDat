﻿using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Characters;
using SimaDat.Models;

namespace SimaDat.Bll
{
    public class PossibilityBll : IPossibilityBll
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public bool RequestDating(Hero h, Girl g)
        {
            float probability = h.Charm * 1.0f / MySettings.MaxCharmForHero;

            return _random.NextDouble() <= probability;
        }
    }
}