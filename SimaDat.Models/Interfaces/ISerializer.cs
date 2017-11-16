using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Interfaces
{
    public interface ISerializer
    {
        Location DeserializeLocation(string jsonLocation);

        string Serialize(Location loc);

        string Serialize(Location.Door door);
    }
}
