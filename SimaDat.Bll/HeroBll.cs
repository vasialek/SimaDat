using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;

namespace SimaDat.Bll
{

    public interface IHeroBll
    {
        void MoveTo(Hero h, Location from, Location to);
    }

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
