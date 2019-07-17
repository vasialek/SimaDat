using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Items;

namespace SimaDat.Bll
{
	public class DatingBll : IDatingBll
    {
        /// <summary>
        /// When girl is ready for kiss
        /// </summary>
        private int _kissLevel = 10;

        public DatingLocation Location { get; private set; }

        
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

            Location = datingLocation;
            Location.Hero = h;
            Location.Girl = g;
        }

        public void Kiss()
        {
            EnsureDatingIsNotOver();

            if (Location.KissPoints < _kissLevel)
            {
                // Girl is little dissapointed
                Location.IncreaseKissPoints(-1);
                throw new BadConditionException("Girl is not ready for kiss");
            }

            // Make her lover
            int likesNeeded = Models.MySettings.Get().GetLikesForFriendships(FriendshipLevels.Lover) - Models.MySettings.Get().GetLikesForFriendships(Location.Girl.FriendshipLevel);
            Location.Girl.LikeHero(likesNeeded);

            // Mark success
            Location.WasKiss = true;
        }

        public void Present(GiftTypes gt)
        {
            EnsureDatingIsNotOver();

            var gift = Location.Hero.Gifts?.FirstOrDefault(x => x.GiftTypeId == gt);
            if (gift == null)
            {
                throw new ObjectDoesNotExistException($"You have no {gt} to present.", (int)gt);
            }

            switch (gift.GiftTypeId)
            {
                case GiftTypes.Flower:
                case GiftTypes.TeddyBear:
                case GiftTypes.DiamondRing:
                    Location.IncreaseKissPoints();
                    break;
                default:
                    break;
            }
            Location.Hero.Gifts.Remove(gift);
        }

        private void EnsureDatingIsNotOver()
        {
            if (Location.WasKiss)
            {
                throw new EventIsOverException($"Dating in {Location.Name} is over, you've already kissed {Location.Girl.Name}");
            }
        }

		public void Present(DatingLocation loc, GiftTypes gift)
		{
			loc.KissPoints++;
		}
	}
}
