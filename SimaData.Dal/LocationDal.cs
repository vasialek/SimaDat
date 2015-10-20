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
            _locations.Add(location);
        }

        public IList<Location> GetAllLocations()
        {
            return _locations;
        }
    }
}
