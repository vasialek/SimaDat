using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;

namespace SimaDat.Bll
{
    public class CharactersBll : ICharactersBll
    {
        private IList<Girl> _girls = new List<Girl>();

        public void CreateGirl(Girl g)
        {
            if (g.CharacterId < 1)
            {
                g.CharacterId = _girls?.Count > 0 ? _girls.Max(x => x.CharacterId) + 1 : 1;
            }
            _girls.Add(g);
        }

        public void SayHi(Hero hero, Girl girl)
        {
            if (hero.CurrentLocationId != girl.CurrentLocationId)
            {
                throw new ObjectNotHereException($"No girl named {girl.Name} in location #{hero.CurrentLocationId}");
            }
            if (hero.Ttl < 1)
            {
                throw new NoTtlException($"You can't say 'Hi' to {girl.Name} because not enough TTL");
            }

            hero.UseTtl(1);
            girl.LikeHero();
        }

        public void Talk(Hero hero, Girl girl)
        {
            if (hero.CurrentLocationId != girl.CurrentLocationId)
            {
                throw new ObjectNotHereException($"No girl named {girl.Name} in location #{hero.CurrentLocationId}");
            }

            switch (girl.FriendshipLevel)
            {
                case Models.Enums.FriendshipLevels.Stranger:
                    break;
                case Models.Enums.FriendshipLevels.SawHimSomewhere:
                    girl.LikeHero();
                    break;
                case Models.Enums.FriendshipLevels.Familar:
                    break;
                case Models.Enums.FriendshipLevels.Friend:
                    break;
                case Models.Enums.FriendshipLevels.Lover:
                    break;
                default:
                    break;
            }
        }

        public IList<Girl> GetAll()
        {
            return _girls;
        }

        public IList<Girl> FindInLocation(int locationId)
        {
            return _girls.Where(x => x.CurrentLocationId == locationId).ToList();
        }
    }
}
