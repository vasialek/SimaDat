﻿using SimaDat.Models.Enums;
using SimaDat.Models.Items;
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

        public virtual string ShortDescription
        {
            get
            {
                return String.Concat(Name, " takes ", TtlToUse, " hours");
            }
        }

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
        public ActionToImprove Bonus { get; private set; } = null;

        /// <summary>
        /// Hero could decrease his sill in some cases
        /// </summary>
        public ActionToImprove Penalty { get; private set; } = null;

        public override string ShortDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("{0} for {1} hours to earn {2}", Name, TtlToUse, MoneyToEarn);
                if (Penalty != null)
                {
                    sb.AppendFormat(". Penalty: {0} to your {1}", Penalty.PointsToImprove, Penalty.SkillToImprove);
                }
                if (Bonus != null)
                {
                    sb.AppendFormat(". Bonus: {0} to your {1}", Bonus.PointsToImprove, Bonus.SkillToImprove);
                }

                return sb.ToString();
            }
        }

        public ActionToWork(string name, int ttl, int money)
            : base(name, ttl)
        {
            MoneyToEarn = money;
        }

        public void SetBonus(HeroSkills skill, int pointsToImprove)
        {
            Bonus = new ActionToImprove("Bonus", skill, 0, pointsToImprove);
        }

        public void SetPenalty(HeroSkills skill, int pointsToLoose)
        {
            Penalty = new Actions.ActionToImprove("Penalty", skill, 0, -pointsToLoose);
        }
    }

    public class ActionToBuy : ActionToDo
    {
        public Gift Gift { get; private set; }

        public override string ShortDescription
        {
            get
            {
                return String.Concat("Buy ", Gift.Name, " for price of ", Gift.Price);
            }
        }

        public ActionToBuy(Gift g)
            : base(g.Name, 0)
        {
            Gift = g;
        }
    }
}
