﻿using SimaDat.Models;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;

namespace SimaDat.Core
{
    public class DatingBll : IDatingBll
    {
        private readonly IProbabilityBll _probabilityBll;

        private readonly int _kissLevel = 10;

        public DatingBll()
            : this(null)
        {
        }

        public DatingBll(IProbabilityBll probabilityBll)
        {
            _probabilityBll = probabilityBll ?? BllFactory.Current.ProbabilityBll;
        }

        public IEnumerable<ActionToDo> GetHeroActions(DatingLocation loc)
        {
            var actions = new List<ActionToDo>();

            actions.Add(new ActionToQuit("Finish dating and quit"));

            actions.Add(new ActionToPresent($"Present {GiftTypes.Flower}", loc.Girl, GiftTypes.Flower));
            actions.Add(new ActionToPresent($"Present {GiftTypes.TeddyBear}", loc.Girl, GiftTypes.TeddyBear));
            actions.Add(new ActionToPresent($"Present {GiftTypes.DiamondRing}", loc.Girl, GiftTypes.DiamondRing));

            actions.Add(new ActionToKiss("Kiss her"));
            //actions.Add(new ActionToKiss("Force her"));

            return actions;
        }

        public int IncreaseKissPoints(DatingLocation datingLocation, int kissPoints)
        {
            datingLocation.KissPoints += kissPoints;
            if (datingLocation.KissPoints > MySettings.MaxKissPoints)
            {
                datingLocation.KissPoints = MySettings.MaxKissPoints;
            }
            else if (datingLocation.KissPoints < 0)
            {
                datingLocation.KissPoints = 0;
            }

            return datingLocation.KissPoints;
        }

        public void JoinDating(Hero h, Girl g, DatingLocation datingLocation)
        {
            if (h.Ttl < 1)
            {
                throw new NoTtlException($"Could not date with {g.Name}, because not enough TTL");
            }
            if (datingLocation.Price > h.Money)
            {
                throw new NoMoneyException($"You can't join date in {datingLocation.Name}, because you have no {datingLocation.Price} to pay.");
            }
            if ((int)g.FriendshipLevel < (int)FriendshipLevels.Friend)
            {
                throw new FriendshipLeveTooLowException($"You can't date {g.Name}, because you are not friends.");
            }

            h.UseTtl(3);
            h.SpendMoney(datingLocation.Price);

            datingLocation.KissPoints = 0;
            datingLocation.Hero = h;
            datingLocation.Girl = g;
        }

        public void Kiss(DatingLocation datingLocation)
        {
            EnsureDatingIsNotOver(datingLocation);

            if (datingLocation.KissPoints < _kissLevel)
            {
                // Girl is little dissapointed
                IncreaseKissPoints(datingLocation, -1);
                throw new BadConditionException("Girl is not ready for kiss");
            }

            // Make her lover
            int likesNeeded = MySettings.Get().GetLikesForFriendships(FriendshipLevels.Lover) - MySettings.Get().GetLikesForFriendships(datingLocation.Girl.FriendshipLevel);
            datingLocation.Girl.LikeHero(likesNeeded);

            // Mark success
            datingLocation.WasKiss = true;
        }

        public void Present(DatingLocation datingLocation, GiftTypes giftType)
        {
            EnsureDatingIsNotOver(datingLocation);

            var gift = datingLocation.Hero.Gifts?.FirstOrDefault(x => x.GiftTypeId == giftType);
            if (gift == null)
            {
                throw new ObjectDoesNotExistException($"You have no {giftType} to present.", (int)giftType);
            }

            switch (gift.GiftTypeId)
            {
                case GiftTypes.Flower:
                case GiftTypes.TeddyBear:
                case GiftTypes.DiamondRing:
                    IncreaseKissPoints(datingLocation, 1);
                    break;

                default:
                    break;
            }
            datingLocation.Hero.Gifts.Remove(gift);
        }

        private static void EnsureDatingIsNotOver(DatingLocation datingLocation)
        {
            if (datingLocation.WasKiss)
            {
                throw new EventIsOverException($"Dating in {datingLocation.Name} is over, you've already kissed {datingLocation.Girl.Name}");
            }
        }
    }
}
