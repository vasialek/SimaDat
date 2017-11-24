using SimaDat.Models.Characters;
using SimaDat.Models.Skills;

namespace SimaDat.Models.Interfaces
{
    public interface IHeroBll
    {
        void MoveTo(Hero h, Location from, Location to);

        void Improve(Hero h, SkillImprovement skill);

        /// <summary>
        /// Should restore TTL to maximum
        /// </summary>
        void Sleep(Hero h);
    }
}
