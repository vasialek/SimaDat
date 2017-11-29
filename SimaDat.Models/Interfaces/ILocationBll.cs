using SimaDat.Models.Actions;
using SimaDat.Models.Enums;
using SimaDat.Models.Skills;
using System.Collections.Generic;

namespace SimaDat.Models.Interfaces
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
        Location GetLocationById(int locationId);

        IList<Location> GetAllLocations();

        // Actions / Characters / Items for location
        IList<SkillImprovement> GetSkillsToImprove(Location location);

        IList<ActionToDo> GetPossibleActions(Location location);

        /*
         * Maintain
         */
        // Clear all locations
        void Clear();
    }
}
