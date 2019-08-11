using SimaDat.Models;
using SimaDat.Models.Enums;

namespace SimaDat.Shared
{
	public static class ProbabilityCalculator
	{
		public static float ProbabilityToKiss(int heroCharm, FriendshipLevels friendshipLevel)
		{
			// [0 - 100]
			float charm = heroCharm * 100f / MySettings.MaxCharmForHero;
			// [0 - 2]
			float friendhsip = ((int)friendshipLevel - (int)FriendshipLevels.Familar);

			// Max probability is 0.8 + 0.18 = 0.98
			return charm * 0.008f + friendhsip * 0.09f;

		}
	}
}
