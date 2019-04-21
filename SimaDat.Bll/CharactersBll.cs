using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Enums;

namespace SimaDat.Bll
{
    public class CharactersBll : ICharactersBll
    {
        private IProbabilityBll _possibilityBll = null;

        private IList<Girl> _girls = new List<Girl>();

        public CharactersBll()
            : this(null)
        {
        }

        public CharactersBll(IProbabilityBll possibilityBll)
        {
            _possibilityBll = possibilityBll ?? BllFactory.Current.ProbabilityBll;
        }

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
            if (hero.Ttl < 1)
            {
                throw new NoTtlException($"You can't talk with {girl.Name} because not enough TTL");
            }

            hero.UseTtl(1);
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

        public Girl GetById(int characterId)
        {
            return _girls.First(x => x.CharacterId == characterId);
        }

        public IList<Girl> GetAll()
        {
            return _girls;
        }

        public IList<Girl> FindInLocation(int locationId)
        {
            return _girls.Where(x => x.CurrentLocationId == locationId).ToList();
        }

        public void Present(Hero h, Girl g, GiftTypes giftTypeId)
        {
            if (h.Ttl < 1)
            {
                throw new NoTtlException($"You can't present gift to {g.Name} because not enough TTL.");
            }

            if (h.CurrentLocationId != g.CurrentLocationId)
            {
                throw new ObjectNotHereException("Could not present gift, because girl is not here.");
            }

            var gift = h.Gifts?.FirstOrDefault(x => x.GiftTypeId == giftTypeId);
            if (gift == null)
            {
                throw new ObjectDoesNotExistException("Hero does not have gift to present.", (int)giftTypeId);
            }

            if ((int)g.FriendshipLevel < (int)FriendshipLevels.Familar)
            {
                throw new FriendshipLeveTooLowException($"Could not present gift, need to reach friendship {FriendshipLevels.Familar}");
            }

            g.LikeHero(gift.FirendshipPoints);
            h.Gifts.Remove(gift);
            h.UseTtl(1);
        }

        public bool AskDating(Hero h, Girl g)
        {
            if (h.Ttl < 1)
            {
                throw new NoTtlException($"Could not ask for dating, because not enough TTL.");
            }
            if (h.CurrentLocationId != g.CurrentLocationId)
            {
                throw new ObjectNotHereException("Could not ask for dating, because girl is not here.");
            }
            if ((int)g.FriendshipLevel < (int)FriendshipLevels.Friend)
            {
                throw new FriendshipLeveTooLowException($"Could not ask for dating, need to reach friendship {FriendshipLevels.Friend}");
            }

            h.UseTtl(1);

            return _possibilityBll.RequestDating(h, g);
        }
    }
}
