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

namespace SimaDat.Bll
{
    public class DatingBll : IDatingBll
    {
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
        
        }
    }
}
