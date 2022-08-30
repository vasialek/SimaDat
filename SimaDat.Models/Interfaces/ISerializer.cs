namespace SimaDat.Models.Interfaces
{
    public interface ISerializer
    {
        Location DeserializeLocation(string json);

        string Serialize(Location location);

        string Serialize(Location.Door door);
    }
}
