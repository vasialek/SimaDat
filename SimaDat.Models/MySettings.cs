using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models
{
    public class MySettings
    {
        public static readonly int MaxTtlForHero = 24;

        public static readonly int MaxIqForHero = 200;
        public static readonly int MaxCharmForHero = 200;
        public static readonly int MaxStrengthForHero = 200;

        public int GetLikesForFriendships(FriendshipLevels levelNeeded)
        {
            switch (levelNeeded)
            {
                case FriendshipLevels.Stranger:
                    return 0;
                case FriendshipLevels.SawHimSomewhere:
                    return 3;
                case FriendshipLevels.Familar:
                    return 7;
                case FriendshipLevels.Friend:
                    return 20;
                case FriendshipLevels.Lover:
                    return 100;
                default:
                    throw new ArgumentOutOfRangeException("Unknown friendship level to reach: " + levelNeeded);
            }
        }

        public static MySettings Get()
        {
            return new MySettings();
        }

    }
}
