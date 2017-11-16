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
    }

    public class LocationDal : ILocationDal
    {
        private IList<Location> _locations = new List<Location>();

        public void CreateLocation(Location location)
        {
            if (location.LocationId < 1)
            {
                location.LocationId = _locations.Max(x => x.LocationId) + 1;
            }
            _locations.Add(location);
        }

        public IList<Location> GetAllLocations()
        {
            return _locations;
        }
    }
}
