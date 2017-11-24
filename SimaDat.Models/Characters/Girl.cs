using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Characters
{
    public class Girl
    {
        protected int[] _likesForFriendship = null;

        public int CharacterId { get; set; }

        public string Name { get; set; }

        public int CurrentLocationId { get; set; }

        public int HeroLikes { get; private set; }

        public Appearance Appearance { get; set; }

        public FriendshipLevels FriendshipLevel { get; private set; } = FriendshipLevels.Stranger;

        public Girl()
        {
            var settings = MySettings.Get();
            _likesForFriendship = new int[5];
            _likesForFriendship[(int)FriendshipLevels.Stranger] = settings.GetLikesForFriendships(FriendshipLevels.Stranger);
            _likesForFriendship[(int)FriendshipLevels.SawHimSomewhere] = settings.GetLikesForFriendships(FriendshipLevels.SawHimSomewhere);
            _likesForFriendship[(int)FriendshipLevels.Familar] = settings.GetLikesForFriendships(FriendshipLevels.Familar);
            _likesForFriendship[(int)FriendshipLevels.Friend] = settings.GetLikesForFriendships(FriendshipLevels.Friend);
            _likesForFriendship[(int)FriendshipLevels.Lover] = settings.GetLikesForFriendships(FriendshipLevels.Lover);
        }

        public void LikeHero()
        {
            HeroLikes++;
            for (int i = _likesForFriendship.Length - 1; i >= 0; i--)
            {
                if (HeroLikes >= _likesForFriendship[i])
                {
                    FriendshipLevel = (FriendshipLevels)i;
                    return;
                }
            }
        }
    }
}
