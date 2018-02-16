using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
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
    }
}
