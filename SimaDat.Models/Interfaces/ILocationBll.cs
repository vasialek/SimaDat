﻿using SimaDat.Models.Enums;
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
        IList<Location> GetAllLocations();
    }
}