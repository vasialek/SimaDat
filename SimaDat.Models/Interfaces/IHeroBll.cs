using SimaDat.Models.Actions;
using SimaDat.Models.Characters;

namespace SimaDat.Models.Interfaces
{
    public interface IHeroBll
    {
        void ApplyAction(Hero hero, ActionToDo action);

        void MoveTo(Hero hero, int fromId, int toId);

        void MoveTo(Hero hero, Location from, Location to);

        void Improve(Hero hero, ActionToImprove skill);

        void Work(Hero hero, ActionToWork job);

        void JumpTo(Hero hero, Location to);

        void Sleep(Hero hero);
    }
}
