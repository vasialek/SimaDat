using SimaDat.Models;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaData.Dal;

namespace SimaDat.Core
{
    public class LocationBll : ILocationBll
    {
        private readonly ICharactersBll _characterBll;
        private readonly ILocationDal _locationDal;

        public LocationBll(ICharactersBll characterBll, ILocationDal locationDal)
            : this(characterBll, locationDal, null)
        {
        }

        public LocationBll(ICharactersBll characterBll, ILocationDal locationDal, IShopBll shopBll)
        {
            _characterBll = characterBll ?? throw new ArgumentNullException(nameof(characterBll));
            _locationDal = locationDal ?? throw new ArgumentNullException(nameof(locationDal));
        }

        public IList<ActionToDo> GetPossibleActions(Location location)
        {
            var actions = new List<ActionToDo>();

            foreach (var d in location.Doors.OrderBy(x => x.Direction))
            {
                var loc = GetLocationById(d.LocationToGoId);
                actions.Add(new ActionToMove($"Move to {d.Direction} for {loc.Name} ({d.LocationToGoId})", d.LocationToGoId));
            }

            return actions;
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
            var oppositeDirection = GetOppositeDirection(doorsAt);
            if (to.GetDoorAtDirection(oppositeDirection) != null)
            {
                throw new DirectionInUseException("There is door from other location to this");
            }

            var d = new Location.Door { LocationToGoId = to.LocationId, Direction = doorsAt };
            from.Doors.Add(d);

            d = new Location.Door { LocationToGoId = @from.LocationId, Direction = oppositeDirection };
            to.Doors.Add(d);
        }

        public void CreateLocation(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }
            var existing = _locationDal.GetAllLocations().FirstOrDefault(x => x.Name == location.Name);
            if (existing != null)
            {
                throw new ArgumentException($"Could not create location with duplicated name `{location.Name}`", "Name");
            }

            _locationDal.CreateLocation(location);
        }

        public Location GetLocationById(int locationId)
        {
            var location = _locationDal.GetAllLocations().FirstOrDefault(x => x.LocationId == locationId);
            if (location == null)
            {
                throw new ObjectDoesNotExistException("Location is not found by #" + locationId, locationId);
            }
            return location;
        }

        public IList<Location> GetAllLocations()
        {
            return _locationDal.GetAllLocations();
        }

        public IList<ActionToDo> GetSkillsToImprove(Location location)
        {
            var actions = new List<ActionToDo>();

            switch (location.Name.ToLower())
            {
                case "gym":
                    actions.Add(new ActionToImprove("Improve your strength", HeroSkills.Strength, 5, 1));
                    break;

                case "pub":
                    actions.Add(new ActionToImprove("Improve charm", HeroSkills.Charm, 3, 1, 10));
                    break;

                case "school":
                    actions.Add(new ActionToImprove("Improve IQ", HeroSkills.Iq, 4, 2));
                    break;

                case "library":
                    actions.Add(new ActionToImprove("Improve IQ", HeroSkills.Iq, 4, 1));
                    break;

                case "home":
                    actions.Add(new ActionToRest());
                    break;

                case "dock":
                    var dw = new ActionToWork("Move boxes", 4, 10);
                    dw.SetBonus(HeroSkills.Strength, 1);
                    actions.Add(dw);
                    break;

                case "bazaar":
                    var bw = new ActionToWork("Sell samshit", 5, 20);
                    bw.SetPenalty(HeroSkills.Iq, 1);
                    actions.Add(bw);

                    var gifts = BllFactory.Current.ShopBll.GetListOfGifts();
                    foreach (var g in gifts)
                    {
                        actions.Add(new ActionToBuy(g));
                    }
                    break;

                case "cafe":
                    var cw = new ActionToWork("Work in cafe", 6, 10);
                    cw.SetBonus(HeroSkills.Charm, 1);
                    actions.Add(cw);
                    break;
            }

            return actions;
        }

        public void Clear()
        {
            _locationDal.Clear();
        }

        public Girl GetOwnerOfLocation(int locationId)
        {
            var loc = GetLocationById(locationId);
            if (loc.OwnerId > 0)
            {
                return _characterBll.GetById(loc.OwnerId);
            }

            return null;
        }
    }
}
