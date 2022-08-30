using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using System.Collections.Generic;

namespace SimaDat.Models.Interfaces
{
    public interface IDatingBll
    {
        void JoinDating(Hero hero, Girl girl, DatingLocation datingLocation);

        void Kiss(DatingLocation datingLocation);

        int IncreaseKissPoints(DatingLocation datingLocation, int kissPoints);

        IEnumerable<ActionToDo> GetHeroActions(DatingLocation datingLocation);

        void Present(DatingLocation datingLocation, GiftTypes gift);
    }
}
