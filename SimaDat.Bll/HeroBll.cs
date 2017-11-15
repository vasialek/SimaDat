using System;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;

namespace SimaDat.Bll
{

    public class HeroBll : IHeroBll
    {
        private ILocationBll _locationBll = null;

        public HeroBll(ILocationBll locationBll)
        {
            if (locationBll == null)
            {
                throw new ArgumentNullException("locationBll");
            }

            _locationBll = locationBll;
        }

        public void MoveTo(Hero h, Location from, Location to)
        {
            if (h.CurrentLocationId != from.LocationId)
            {
                throw new ObjectNotHereException("Hero is not in location you want to move from");
            }

            if (_locationBll.CouldMoveTo(from, to) == false)
            {
                throw new CouldNotMoveException("There is no door to desired location");
            }

            h.CurrentLocationId = to.LocationId;
        }
    }
}
