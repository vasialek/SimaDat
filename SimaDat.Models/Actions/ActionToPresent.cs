using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Actions
{
    public class ActionToPresent : ActionToDo
    {
		public GiftTypes GiftType { get; private set; }

		public ActionToPresent(string name, Girl g, GiftTypes giftType)
            : base(name, 1)
        {
			GiftType = giftType;
        }
    }
}
