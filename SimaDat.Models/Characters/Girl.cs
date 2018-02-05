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
            : this(null, FriendshipLevels.Stranger)
        {
        }

        public Girl(string name)
            : this(name, FriendshipLevels.Stranger)
        {
        }

        public Girl(string name, FriendshipLevels friendLevel)
        {
            Name = name;
            FriendshipLevel = friendLevel;

            var settings = MySettings.Get();
            _likesForFriendship = new int[5];
            _likesForFriendship[(int)FriendshipLevels.Stranger] = settings.GetLikesForFriendships(FriendshipLevels.Stranger);
            _likesForFriendship[(int)FriendshipLevels.SawHimSomewhere] = settings.GetLikesForFriendships(FriendshipLevels.SawHimSomewhere);
            _likesForFriendship[(int)FriendshipLevels.Familar] = settings.GetLikesForFriendships(FriendshipLevels.Familar);
            _likesForFriendship[(int)FriendshipLevels.Friend] = settings.GetLikesForFriendships(FriendshipLevels.Friend);
            _likesForFriendship[(int)FriendshipLevels.Lover] = settings.GetLikesForFriendships(FriendshipLevels.Lover);

            HeroLikes = _likesForFriendship[(int)friendLevel];
        }

        public void LikeHero(int likes = 1)
        {
            HeroLikes += likes;
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
