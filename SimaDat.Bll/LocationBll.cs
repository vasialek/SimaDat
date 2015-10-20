﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaData.Dal;

namespace SimaDat.Bll
{

    public interface ILocationBll
    {
        bool CouldMoveTo(Location from, Location to);

        // Should return Directions.South if Directions.North is passed
        Directions GetOppositeDirection(Directions d);

        // Modification of location

        void CreateLocation(Location location);

        void CreateDoorInLocation(Location from, Location to, Directions doorsAt);

        // Navigation/search
        IList<Location> GetAllLocations();
    }

    public class LocationBll : ILocationBll
    {

        private ILocationDal _locationDal = null;

        public LocationBll(ILocationDal locationDal)
        {
            if (locationDal == null)
            {
                throw new ArgumentNullException(nameof(locationDal));
            }

            _locationDal = locationDal;
        }

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

        public Directions GetOppositeDirection(Directions d)
        {
            Dictionary<Directions, Directions> ar = new Dictionary<Directions, Directions>();
            ar[Directions.North] = Directions.South;
            ar[Directions.South] = Directions.North;
            ar[Directions.East] = Directions.West;
            ar[Directions.West] = Directions.East;
            ar[Directions.NorthEast] = Directions.SouthWest;
            ar[Directions.NorthWest] = Directions.SouthEast;
            ar[Directions.SouthEast] = Directions.NorthWest;
            ar[Directions.SouthWest] = Directions.NorthEast;

            if (ar.ContainsKey(d) == false)
            {
                throw new ArgumentException("Could not get opposite direction for: " + d);
            }
            return ar[d];
        }

        public void CreateDoorInLocation(Location from, Location to, Directions doorsAt)
        {
            // Ensure there is no door at desired direction
            if (from.GetDoorAtDirection(doorsAt) != null)
            {
                throw new DirectionInUseException("There is already door at direction: " + doorsAt);
            }
            // Ensure we could go back in same door and no door exists
            Directions oppositeDirection = GetOppositeDirection(doorsAt);
            if (to.GetDoorAtDirection(oppositeDirection) != null)
            {
                throw new DirectionInUseException("There is door from other location to this");
            }

            var d = new Location.Door();
            d.LocationToGoId = to.LocationId;
            d.Direction = doorsAt;
            from.Doors.Add(d);

            d = new Location.Door();
            d.LocationToGoId = from.LocationId;
            d.Direction = oppositeDirection;
            to.Doors.Add(d);
        }

        public void CreateLocation(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            _locationDal.CreateLocation(location);
        }

        public IList<Location> GetAllLocations()
        {
            return _locationDal.GetAllLocations();
        }
    }
}
