using AutoMapper;
using Newtonsoft.Json;
using SimaDat.Models;
using SimaDat.Models.Enums;
using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace SimaDat.Bll
{
    public class SerializerBll : ISerializer
    {
        public Location DeserializeLocation(string jsonLocation)
        {
            AutoMapperInitializer.Initialize();
            var sl = JsonConvert.DeserializeObject<SerializedLocation>(jsonLocation);

            var loc = Mapper.Map<Location>(sl);

            // Mapping of string[] => Door is bad, so do it manually
            loc.Doors.Clear();
            for (int i = 0; i < sl.doors?.Count; i++)
            {
                loc.Doors.Add(new Location.Door
                {
                    LocationToGoId = Convert.ToInt32(sl.doors[i][1]),
                    Direction = CodeToDirection(sl.doors[i][0])
                });
            }

            return loc;
        }

        public string Serialize(Location loc)
        {
            AutoMapperInitializer.Initialize();
            var sl = Mapper.Map<SerializedLocation>(loc);
            return JsonConvert.SerializeObject(sl);
        }

        public string Serialize(Location.Door door)
        {
            return $"[\"{DirectionToCode(door.Direction)}\",\"{door.LocationToGoId}\"]";
        }

        public static string DirectionToCode(Directions d)
        {
            switch (d)
            {
                case Directions.North:
                    return "N";

                case Directions.South:
                    return "S";

                case Directions.East:
                    return "E";

                case Directions.West:
                    return "W";

                case Directions.NorthEast:
                    return "NE";

                case Directions.NorthWest:
                    return "NW";

                case Directions.SouthEast:
                    return "SE";

                case Directions.SouthWest:
                    return "SW";

                case Directions.Top:
                    return "T";

                case Directions.Bottom:
                    return "B";

                default:
                    throw new ArgumentOutOfRangeException($"Could not translate Direction `{d}` to code");
            }
        }

        public static Directions CodeToDirection(string directionCode)
        {
            switch (directionCode.ToUpper())
            {
                case "N":
                    return Directions.North;

                case "S":
                    return Directions.South;

                case "E":
                    return Directions.East;

                case "W":
                    return Directions.West;

                case "NE":
                    return Directions.NorthEast;

                case "NW":
                    return Directions.NorthWest;

                case "SE":
                    return Directions.SouthEast;

                case "SW":
                    return Directions.SouthWest;

                case "T":
                    return Directions.Top;

                case "B":
                    return Directions.Bottom;
            }
            throw new ArgumentOutOfRangeException($"Could not translate direction `{directionCode}`");
        }
    }

    internal class SerializedLocation
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<string[]> doors { get; set; }
    }
}
