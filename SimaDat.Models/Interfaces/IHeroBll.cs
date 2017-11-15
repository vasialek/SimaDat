using SimaDat.Models.Characters;

namespace SimaDat.Models.Interfaces
{
    public interface IHeroBll
    {
        void MoveTo(Hero h, Location from, Location to);
    }
}
