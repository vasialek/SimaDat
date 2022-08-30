using SimaDat.Models.Enums;
using SimaDat.Models.Items;
using System.Text;

namespace SimaDat.Models.Actions
{
    public class ActionToDo
    {
        public bool DoesImpactHero { get; protected set; }

        public int TtlToUse { get; protected set; }

        public string Name { get; protected set; }

        public virtual string ShortDescription => $"{Name} takes {TtlToUse} hours";

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
        public HeroSkills SkillToImprove { get; }

        public int PointsToImprove { get; }

        public int MoneyToSpent { get; }

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

        public override string ShortDescription => "Sleep to restore TTL to max";
    }

    public class ActionToWork : ActionToDo
    {
        public int MoneyToEarn { get; }

        /// <summary>
        /// Hero could increase his skill (if != null)
        /// </summary>
        public ActionToImprove Bonus { get; private set; }

        /// <summary>
        /// Hero could decrease his sill in some cases
        /// </summary>
        public ActionToImprove Penalty { get; private set; }

        public override string ShortDescription
        {
            get
            {
                var sb = new StringBuilder();

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
        public Gift Gift { get; }

        public override string ShortDescription => $"Buy {Gift.Name} for price of {Gift.Price}";

        public ActionToBuy(Gift gift)
            : base(gift.Name, 0)
        {
            Gift = gift;
        }
    }
}
