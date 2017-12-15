using System;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Actions;

namespace SimaDat.Bll
{

    public class HeroBll : IHeroBll
    {
        private ILocationBll _locationBll = null;

        public HeroBll(ILocationBll locationBll)
        {
            if (locationBll == null)
            {
                throw new ArgumentNullException("locationBll");
            }

            _locationBll = locationBll;
        }

        public void ApplyAction(Hero h, ActionToDo action)
        {
            var ma = action as ActionToMove;
            ActionToImprove ia = action as ActionToImprove;
            ActionToRest sa = action as ActionToRest;
            ActionToWork wa = action as ActionToWork;
            if (ma != null)
            {
                MoveTo(h, h.CurrentLocationId, ma.LocationIdToGo);
                return;
            }
            if (ia != null)
            {
                Improve(h, ia);
                return;
            }
            if (sa != null)
            {
                Sleep(h);
                return;
            }
            if (wa != null)
            {
                Work(h, wa);
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(action), $"Unknown action to apply to Hero: {action.Name} ({action.GetType()}).");
        }

        public void Work(Hero h, ActionToWork job)
        {
            if (h.Ttl < job.TtlToUse)
            {
                throw new NoTtlException($"Could not work {job.Name} - not enough TTL. {h.Ttl} of {job.TtlToUse} needed.");
            }

            h.UseTtl(job.TtlToUse);
            h.SpendMoney(-job.MoneyToEarn);
        }

        public void Improve(Hero h, ActionToImprove skill)
        {
            if (h.Ttl < skill.TtlToUse)
            {
                throw new NoTtlException($"Could not improve {skill.SkillToImprove} - not enough TTL. {h.Ttl} of {skill.TtlToUse} needed.");
            }

            if (h.Money < skill.MoneyToSpent)
            {
                throw new NoMoneyException($"Could not improve {skill.SkillToImprove} - not enough money. {h.Money} of {skill.MoneyToSpent} needed.");
            }

            h.SpendMoney(skill.MoneyToSpent);
            h.ModifySkill(skill.SkillToImprove, skill.PointsToImprove);
            h.UseTtl(skill.TtlToUse);
        }

        public void MoveTo(Hero h, int fromId, int toId)
        {
            Location from = _locationBll.GetLocationById(fromId);
            Location to = _locationBll.GetLocationById(toId);

            MoveTo(h, from, to);
        }

        public void MoveTo(Hero h, Location from, Location to)
        {
            if (h.CurrentLocationId != from.LocationId)
            {
                throw new ObjectNotHereException("Hero is not in location you want to move from");
            }

            if (_locationBll.CouldMoveTo(from, to) == false)
            {
                throw new CouldNotMoveException("There is no door to desired location");
            }

            if (h.Ttl < 1)
            {
                throw new NoTtlException("Hero has not enough TTL to move");
            }

            h.CurrentLocationId = to.LocationId;
        }

        /// <summary>
        /// Lets Hero to jump to any location. 1 TTL is used
        /// </summary>
        public void JumpTo(Hero h, Location to)
        {
            if (h.Ttl < 1)
            {
                throw new NoTtlException("Hero has not enough TTL to jump");
            }

            h.UseTtl(1);
            h.CurrentLocationId = to.LocationId;
        }


        public void Sleep(Hero h)
        {
            h.ResetTtl();
        }
    }
}
