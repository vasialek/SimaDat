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
using SimaDat.Models.Actions;

namespace SimaDat.Bll
{
    public class DatingBll : IDatingBll
    {
        public IEnumerable<ActionToDo> GetHeroActions(DatingLocation loc)
        {
            var actions = new List<ActionToDo>();

            actions.Add(new ActionToQuit("Finish dating and quit"));

            actions.Add(new ActionToPresent($"Present {GiftTypes.Flower}", loc.Girl, GiftTypes.Flower));
            actions.Add(new ActionToPresent($"Present {GiftTypes.TeddyBear}", loc.Girl, GiftTypes.TeddyBear));
            actions.Add(new ActionToPresent($"Present {GiftTypes.DiamondRing}", loc.Girl, GiftTypes.DiamondRing));

            actions.Add(new ActionToKiss("Kiss her"));

            return actions;
        }

        public void JoinDating(Hero h, Girl g, DatingLocation loc)
        {
            if (h.Ttl < 1)
            {
                throw new NoTtlException($"Could not date with {g.Name}, because not enough TTL");
            }
            if (loc.Price > h.Money)
            {
                throw new NoMoneyException($"You can't join date in {loc.Name}, because you have no {loc.Price} to pay.");
            }
            if ((int)g.FriendshipLevel < (int)FriendshipLevels.Friend)
            {
                throw new FriendshipLeveTooLowException($"You can't date {g.Name}, because you are not friends.");
            }

            h.UseTtl(1);
            h.SpendMoney(loc.Price);
        }
    }
}
