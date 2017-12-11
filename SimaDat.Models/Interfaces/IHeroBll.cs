using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Skills;

namespace SimaDat.Models.Interfaces
{
    public interface IHeroBll
    {
        void ApplyAction(Hero h, Actions.ActionToDo action);

        void MoveTo(Hero h, int fromId, int toId);

        void MoveTo(Hero h, Location from, Location to);

        void Improve(Hero h, ActionToImprove skill);

        /// <summary>
        /// Should restore TTL to maximum
        /// </summary>
        void Sleep(Hero h);
    }
}
