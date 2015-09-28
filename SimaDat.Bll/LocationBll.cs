using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;

namespace SimaDat.Bll
{

    public interface ILocationBll
    {
        bool CouldMoveTo(Location from, Location to);

        // Modification of location
        void CreateDoorInLocation(Location from, Location to, Directions doorsAt);
    }

    public class LocationBll : ILocationBll
    {

        public bool CouldMoveTo(Location from, Location to)
        {
            var door = from.Doors.FirstOrDefault(x => x.LocationToGoId == to.LocationId);
            if (door != null)
            {
                // TODO: add conditions if necessary
                return true;
            }
            return false;
        }


        public void CreateDoorInLocation(Location from, Location to, Directions doorsAt)
        {
            // Ensure there is no door at desired direction
            if (from.GetDoorAtDirection(doorsAt) != null)
            {
                throw new DirectionInUseException("There is already door at direction: " + doorsAt);
            }

            var d = new Location.Door();
            d.LocationToGoId = to.LocationId;
            d.Direction = doorsAt;

            from.Doors.Add(d);
        }
    }
}
