using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Interfaces;
using System;

namespace SimaDat.Bll
{
	public class ProbabilityBll : IProbabilityBll
	{
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public bool RequestDating(Hero h, Girl g)
        {
            float probability = h.Charm * 1.0f / MySettings.MaxCharmForHero;

            return _random.NextDouble() <= probability;
        }
    }
}
