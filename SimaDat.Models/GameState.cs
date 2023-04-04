using SimaDat.Models.Characters;

namespace SimaDat.Models;

public class GameState
{
    public Hero Hero { get; set; }

    public List<Location> Locations { get; set; }

    public List<Girl> Girls { get; set; }
}
