using SimaDat.Models;
using SimaDat.Models.Enums;

namespace SimaDat.Shared
{
    public static class ProbabilityCalculator
    {
        private const float SmallestProbability = 0.0005f;

        public static float ProbabilityToKiss(int heroCharm, FriendshipLevels friendshipLevel)
        {
            // Probability to kiss w/o any kiss points (got during dating) is very small
            return ProbabilityToKiss(heroCharm, friendshipLevel, 0);
        }

        public static float ProbabilityToKiss(int heroCharm, FriendshipLevels friendshipLevel, int kissPoints)
        {
            // [0 - 100]
            float charm = heroCharm * 100f / MySettings.MaxCharmForHero;
            // [0 - 2]
            float friendship = ((int)friendshipLevel - (int)FriendshipLevels.Familar);

            // Max probability is 0.016 + 0.004 = 0.02
            float p = charm * 0.00016f + friendship * 0.002f;

            return p < SmallestProbability ? SmallestProbability : p;
        }
    }
}
