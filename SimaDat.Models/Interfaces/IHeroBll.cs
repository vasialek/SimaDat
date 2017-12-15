using SimaDat.Models.Actions;
using SimaDat.Models.Characters;

namespace SimaDat.Models.Interfaces
{
    public interface IHeroBll
    {
        void ApplyAction(Hero h, Actions.ActionToDo action);

        void MoveTo(Hero h, int fromId, int toId);

        void MoveTo(Hero h, Location from, Location to);

        void Improve(Hero h, ActionToImprove skill);

        void Work(Hero h, ActionToWork job);

        /// <summary>
        /// Lets Hero to jump to any location. 1 TTL is used
        /// </summary>
        void JumpTo(Hero h, Location to);

        /// <summary>
        /// Should restore TTL to maximum
        /// </summary>
        void Sleep(Hero h);
    }
}
