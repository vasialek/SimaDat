using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Interfaces
{
    public interface IDatingBll
    {
        void JoinDating(Hero h, Girl g, DatingLocation datingLocation);

        void Kiss(DatingLocation datingLocation);

		int IncreaseKissPoints(DatingLocation datingLocation, int kissPoints);

        IEnumerable<ActionToDo> GetHeroActions(DatingLocation loc);

		void Present(DatingLocation loc, GiftTypes gift);
    }
}
