using System;
using System.Collections.Generic;
using System.Linq;
using SimaDat.Models;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaData.Dal;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Skills;

namespace SimaDat.Bll
{

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

        public Location GetLocationById(int locationId)
        {
            return _locationDal.GetAllLocations().First(x => x.LocationId == locationId);
        }

        public IList<Location> GetAllLocations()
        {
            return _locationDal.GetAllLocations();
        }

        public IList<SkillImprovement> GetSkillsToImprove(Location location)
        {
            switch (location.Name.ToLower())
            {
                case "gym":
                    return new SkillImprovement[] { new SkillImprovement { Skill = HeroSkills.Strength, ImprovementPoints = 5, TtlToUse = 20 } };
                case "school":
                    return new SkillImprovement[] { new SkillImprovement { Skill = HeroSkills.Iq, ImprovementPoints = 1, TtlToUse = 40 } };
                case "pub":
                    return new SkillImprovement[] { new SkillImprovement { Skill = HeroSkills.Charm, ImprovementPoints = 2, TtlToUse = 40 } };

            }
            return null;
        }
    }
}
