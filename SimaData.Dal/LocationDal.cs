using SimaDat.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimaData.Dal
{
    public interface ILocationDal
    {
        IList<Location> GetAllLocations();

        void CreateLocation(Location location);

        void Clear();
    }

    public class LocationDal : ILocationDal
    {
        private IList<Location> _locations = new List<Location>();

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
