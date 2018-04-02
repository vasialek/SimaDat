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
        DatingLocation Location { get; }

        void JoinDating(Hero h, Girl g, DatingLocation datingLocation);

        void Present(GiftTypes gt);

        void Kiss();
    }
}
