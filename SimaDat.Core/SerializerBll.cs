using Newtonsoft.Json;
using SimaDat.Models;
using SimaDat.Models.Enums;
using SimaDat.Models.Interfaces;

namespace SimaDat.Core
{
    public class SerializerBll : ISerializer
    {
        public Location DeserializeLocation(string json)
        {
            return JsonConvert.DeserializeObject<Location>(json);
        }

        public string Serialize(Location location)
        {
            return JsonConvert.SerializeObject(JsonConvert.SerializeObject(location));
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
}
