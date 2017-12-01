using System;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Skills;
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
            if (ma != null)
            {
                MoveTo(h, h.CurrentLocationId, ma.LocationIdToGo);
                return;
            }
            if (ia != null)
            {
                Improve(h, new SkillImprovement { Skill = ia.SkillToImprove, TtlToUse = ia.TtlToUse, ImprovementPoints = ia.PointsToImprove });
                return;
            }
            if (sa != null)
            {
                Sleep(h);
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(action), $"Unknown action to apply to Hero: {action.Name} ({action.GetType()})");
        }

        public void Improve(Hero h, SkillImprovement skill)
        {
            if (h.Ttl < skill.TtlToUse)
            {
                throw new NoTtlException($"Could not improve {skill.Skill} - not enough TTL {h.Ttl} of {skill.TtlToUse}");
            }

            h.ModifySkill(skill.Skill, skill.ImprovementPoints);
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

        public void Sleep(Hero h)
        {
            h.ResetTtl();
        }
    }
}
