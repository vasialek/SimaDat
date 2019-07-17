using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using System;

namespace SimaDat.Bll
{
	public class ProbabilityBll : IProbabilityBll
	{
		private static Random _random = new Random((int)DateTime.Now.Ticks);

		public bool Kiss(DatingLocation datingLocation)
		{
			if (datingLocation.IsOver)
			{
				throw new ObjectDoesNotExistException($"Dating with {datingLocation.Girl.Name} in {datingLocation.Name} is over", datingLocation.DatingLocationId);
			}

			// [0 - 100]
			float charm = datingLocation.Hero.Charm * 100f / MySettings.MaxCharmForHero;
			// [0 - 2]
			float friendhsip = ((int)datingLocation.Girl.FriendshipLevel - (int)FriendshipLevels.Familar);

			float probability = charm * 0.008f + friendhsip * 0.09f;

			return _random.NextDouble() <= probability;
		}

		public bool RequestDating(Hero h, Girl g)
		{
			float probability = h.Charm * 1.0f / MySettings.MaxCharmForHero;

			return _random.NextDouble() <= probability;
		}
	}
}
