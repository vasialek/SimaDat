using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Actions
{
    public class ActionToDo
    {
        public bool DoesImpactHero { get; protected set; }

        public int TtlToUse { get; protected set; }

        public string Name { get; protected set; }

        public ActionToDo(string name, int ttlToUse)
        {
            Name = name;
            TtlToUse = ttlToUse;
        }
    }

    public class ActionToMove : ActionToDo
    {
        public int LocationIdToGo { get; protected set; }

        public ActionToMove(string name, int locationIdToGo)
            : base(name, 0)
        {
            DoesImpactHero = true;
            LocationIdToGo = locationIdToGo;
        }
    }

    public class ActionToImprove : ActionToDo
    {
        public HeroSkills SkillToImprove { get; private set; }

        public int PointsToImprove { get; private set; }

        public int MoneyToSpent { get; private set; }

        public ActionToImprove(string name, HeroSkills skill, int ttlToUse, int pointsToImprove, int money = 0)
            : base(name, ttlToUse)
        {
            SkillToImprove = skill;
            PointsToImprove = pointsToImprove;
            MoneyToSpent = money;
        }
    }

    public class ActionToRest : ActionToDo
    {
        public ActionToRest()
            : base("Sleep", 0)
        {
            DoesImpactHero = true;
        }
    }

    public class ActionToWork : ActionToDo
    {
        public int MoneyToEarn { get; private set; }

        /// <summary>
        /// Hero could increase his skill (if != null)
        /// </summary>
        private ActionToImprove _improvement = null;

        /// <summary>
        /// Hero could decrease his sill in some cases
        /// </summary>
        private ActionToImprove _penalty = null;

        public ActionToWork(string name, int ttl, int money)
            : base(name, ttl)
        {
            MoneyToEarn = money;
        }

        public void SetImprovement(HeroSkills skill, int pointsToImprove)
        {

        }

        public void SetPenalty(HeroSkills skill, int pointsToLoose)
        {

        }
    }
}
