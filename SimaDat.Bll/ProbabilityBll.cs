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
		//private static Random _random = new Random((int)DateTime.Now.Ticks);
		private readonly IRandomProvider _randomProvider = null;

		public ProbabilityBll(IRandomProvider randomProvider = null)
		{
			_randomProvider = randomProvider ?? BllFactory.Current.RandomProvider;
		}

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

			// Max probability is 0.8 + 0.18 = 0.98
			float probability = charm * 0.008f + friendhsip * 0.09f;

			return _randomProvider.NextDouble() <= probability;
		}

		public bool RequestDating(Hero h, Girl g)
		{
			// Leave 1% for negative on max charm
			float probability = h.Charm * 0.99f / MySettings.MaxCharmForHero;

			return _randomProvider.NextDouble() <= probability;
		}
	}
}
