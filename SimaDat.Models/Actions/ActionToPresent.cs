using SimaDat.Models.Characters;
using SimaDat.Models.Enums;

namespace SimaDat.Models.Actions
{
    public class ActionToPresent : ActionToDo
    {
        public GiftTypes GiftType { get; }

        public ActionToPresent(string name, Girl girl, GiftTypes giftType)
            : base(name, 1)
        {
            GiftType = giftType;
        }
    }
}
