using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimaDat.Models;

namespace SimaData.Dal
{

    public interface ILocationDal
    {
        IList<Location> GetAllLocations();

        void CreateLocation(Location location);

        /// <summary>
        /// Removes all locations
        /// </summary>
        void Clear();
    }

    public class LocationDal : ILocationDal
    {
        private IList<Location> _locations = new List<Location>();

        /// <summary>
        /// Removes all locations
        /// </summary>
        public void Clear()
        {
            _locations?.Clear();
        }

        public void CreateLocation(Location location)
        {
            if (location.LocationId < 1)
            {
                location.LocationId = _locations?.Count > 0 ? _locations.Max(x => x.LocationId) + 1 : 1;
            }
            _locations.Add(location);
        }

        public IList<Location> GetAllLocations()
        {
            return _locations;
        }
    }
}
