using SimaDat.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Interfaces
{
    public interface IPossibilityBll
    {
        bool RequestDating(Hero h, Girl g);
    }
}
