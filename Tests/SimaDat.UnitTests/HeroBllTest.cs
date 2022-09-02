using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Core;
using SimaDat.Models;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Items;
using SimaDat.UnitTests.FakeClasses;
using SimaData.Dal;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class HeroBllTest
    {
        private Hero _hero;
        private readonly IHeroBll _heroBll = BllFactory.Current.HeroBll;
        private readonly ILocationBll _locationBll = new LocationBll(BllFactory.Current.CharactersBll, DalFactory.Current.LocationDal);

        private readonly Location _from = new Location(1, "From");
        private readonly Location _to = new Location(2, "To");

        private ActionToImprove _improveIq;
        private ActionToImprove _improveStrength;
        private ActionToImprove _improveCharm;
        private ActionToRest _sleepAction;
        private ActionToWork _workAction;
        private ActionToBuy _buyAction;

        [TestInitialize]
        public void TestInit()
        {
            _hero = new Hero
            {
                Name = "Test hero"
            };
            _hero.ResetTtl();

            _improveIq = new ActionToImprove("Iq", HeroSkills.Iq, 4, 1);
            _improveStrength = new ActionToImprove("Strength", HeroSkills.Strength, 5, 2);
            _improveCharm = new ActionToImprove("Charm", HeroSkills.Charm, 6, 3, 100);
            _sleepAction = new ActionToRest();
            _workAction = new ActionToWork("Dock", 4, 10);
            _buyAction = new ActionToBuy(new Gift { GiftId = 1, Name = "Flower", Price = 1, FirendshipPoints = 100 });

            _locationBll.Clear();
            _locationBll.CreateDoorInLocation(_from, _to, Directions.North);
            _locationBll.CreateLocation(_from);
            _locationBll.CreateLocation(_to);

            _hero.CurrentLocationId = _from.LocationId;
        }

        #region Movement

        [TestMethod]
        public void MoveTo_ChangeCurrentLocationId()
        {
            _heroBll.MoveTo(_hero, _from, _to);

            // Expecting that movement is OK
            _hero.CurrentLocationId.Should().Be(_to.LocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotMoveException))]
        public void MoveTo_Exception_WhenMovingWithoutDoor()
        {
            var from = new Location();
            var to = new Location();
            _hero.CurrentLocationId = from.LocationId;

            // Expecting exception, that movement is not possible w/o door
            _heroBll.MoveTo(_hero, from, to);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void MoveTo_Exception_WhenHeroIsNotInLocationFrom()
        {
            _hero.CurrentLocationId = _from.LocationId + 100;

            // Expecting exception moving from wrong location
            _heroBll.MoveTo(_hero, _from, _to);
        }

        [TestMethod]
        public void MoveTo_BadLocationIdFrom()
        {
            var isOk = false;

            try
            {
                _heroBll.MoveTo(_hero, 666666, _to.LocationId);
            }
            catch (ObjectDoesNotExistException odnex) when (odnex.ObjectId == 666666)
            {
                isOk = true;
            }

            isOk.Should().BeTrue("should be error that location to move from is incorrect.");
        }

        [TestMethod]
        public void MoveTo_BadLocationIdTo()
        {
            var isOk = false;

            try
            {
                _heroBll.MoveTo(_hero, _from.LocationId, 777777);
            }
            catch (ObjectDoesNotExistException odnex) when (odnex.ObjectId == 777777)
            {
                isOk = true;
            }

            isOk.Should().BeTrue("should be error that location to move to is incorrect.");
        }

        #endregion Movement

        #region Movement with enter condition

        [TestMethod]
        [ExpectedException(typeof(BadConditionException))]
        public void MoveTo_EnterIsNotPossible()
        {
            // Never allow to enter
            _to.SetEnterCondition("XEP BAM", h => false);

            _heroBll.MoveTo(_hero, _from, _to);
        }

        [TestMethod]
        public void MoveTo_CouldNotEnterOnSaturday()
        {
            var isOk = false;
            var violateMessage = "Closed on weekend";
            // Could not enter after Friday
            _to.SetEnterCondition(violateMessage, h => h.Calendar.WeekDay < 6);
            // Set Hero day to Saturday
            while (_hero.Calendar.WeekDay < 6)
            {
                _heroBll.Sleep(_hero);
            }

            try
            {
                _heroBll.MoveTo(_hero, _from, _to);
            }
            catch (BadConditionException bcex) when (bcex.Message == violateMessage)
            {
                isOk = true;
            }

            isOk.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(BadConditionException))]
        public void MoveTo_CouldNoEnterWhenStranger()
        {
            var girl = new Girl("Laura");
            BllFactory.Current.CharactersBll.CreateGirl(girl);
            _to.OwnerId = girl.CharacterId;
            _to.SetEnterCondition($"You should reach next level of friendship to enter home of {girl.Name}.",
                h => ((int)girl.FriendshipLevel >= (int)FriendshipLevels.SawHimSomewhere));

            _heroBll.MoveTo(_hero, _from, _to);
        }

        #endregion Movement with enter condition

        #region Jump to

        [TestMethod]
        public void JumpTo_TtlDecreaseToOne()
        {
            var v = _hero.Ttl;

            _heroBll.JumpTo(_hero, _to);

            _hero.Ttl.Should().Be(v - 1);
        }

        [TestMethod]
        public void JumpTo_LocationIsChanged()
        {
            _heroBll.JumpTo(_hero, _to);

            _hero.CurrentLocationId.Should().Be(_to.LocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void JumpTo_Exception_WhenNoTtl()
        {
            _hero.UseTtl(MySettings.MaxTtlForHero);

            _heroBll.JumpTo(_hero, _to);
        }

        #endregion Jump to

        #region Improvement

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Improve_HeroShouldHaveTtl()
        {
            var hero = new HeroProxy();
            hero.SetTtl(0);

            // Expecting exception when no TTL to improve
            _heroBll.Improve(hero, _improveIq);
        }

        [TestMethod]
        public void Improve_TtlIsReducedAfterImprovement()
        {
            _heroBll.Sleep(_hero);

            _heroBll.Improve(_hero, _improveIq);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero - _improveIq.TtlToUse);
        }

        [TestMethod]
        public void Improve_IqShouldIncrease()
        {
            _heroBll.Sleep(_hero);
            var v = _hero.Iq;

            _heroBll.Improve(_hero, _improveIq);

            _hero.Iq.Should().Be(v + _improveIq.PointsToImprove);
        }

        [TestMethod]
        public void Improve_StrengthShouldIncrease()
        {
            _heroBll.Sleep(_hero);
            var v = _hero.Strength;

            _heroBll.Improve(_hero, _improveStrength);

            _hero.Strength.Should().Be(v + _improveStrength.PointsToImprove);
        }

        [TestMethod]
        [ExpectedException(typeof(NoMoneyException))]
        public void Improve_NoMoneyException_Charm()
        {
            _heroBll.Sleep(_hero);
            var v = _hero.Charm;

            _heroBll.Improve(_hero, _improveCharm);
        }

        [TestMethod]
        public void Improve_CharmShouldIncrease()
        {
            _heroBll.Sleep(_hero);
            _hero.SpendMoney(-_improveCharm.MoneyToSpent);
            var v = _hero.Charm;

            _heroBll.Improve(_hero, _improveCharm);

            _hero.Charm.Should().Be(v + _improveCharm.PointsToImprove);
        }

        [TestMethod]
        public void Improve_MoneyShouldDecrease_Charm()
        {
            _heroBll.Sleep(_hero);
            _hero.SpendMoney(-1000);
            var v = _hero.Money;

            _heroBll.Improve(_hero, _improveCharm);

            // Expecting money to be decresead on cost of charm
            _hero.Money.Should().Be(v - _improveCharm.MoneyToSpent);
        }

        #endregion Improvement

        #region Sleep

        [TestMethod]
        public void Sleep_ShouldRestoreMaxTtl()
        {
            _heroBll.Sleep(_hero);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero);
        }

        [TestMethod]
        public void Sleep_ChangeDay()
        {
            var v = _hero.Calendar.Day;

            _heroBll.Sleep(_hero);

            // Should be next day after sleep
            _hero.Calendar.Day.Should().BeGreaterThan(v);
        }

        #endregion Sleep

        #region Work

        [TestMethod]
        public void Work_DescreaseTtl()
        {
            var v = _hero.Ttl;

            _heroBll.Work(_hero, _workAction);

            _hero.Ttl.Should().Be(v - _workAction.TtlToUse);
        }

        [TestMethod]
        public void Work_EarnMoney()
        {
            var v = _hero.Money;

            _heroBll.Work(_hero, _workAction);

            _hero.Money.Should().Be(v + _workAction.MoneyToEarn);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Work_Exception_WhenNoTtl()
        {
            _hero.UseTtl(MySettings.MaxTtlForHero);

            _heroBll.Work(_hero, _workAction);
        }

        #endregion Work

        #region Penalty for work/job

        [TestMethod]
        public void Work_DecreaseIq_WhenPenaltyIs()
        {
            _hero.ModifySkill(HeroSkills.Iq, 100);
            _workAction.SetPenalty(HeroSkills.Iq, 1);

            _heroBll.Work(_hero, _workAction);

            // Expecting IQ to decrease by penalty
            _hero.Iq.Should().Be(99);
        }

        [TestMethod]
        public void Work_NoDecreaseIq_WhenIqIsZero()
        {
            _workAction.SetPenalty(HeroSkills.Iq, 1000);

            _heroBll.Work(_hero, _workAction);

            // Do not decrease IQ below 0
            _hero.Iq.Should().BeGreaterOrEqualTo(0);
        }

        #endregion Penalty for work/job

        #region Bonus for job/work

        [TestMethod]
        public void Work_IncreaseCharm_WhenBonusIs()
        {
            var bonus = 1;
            var v = _hero.Charm;
            _workAction.SetBonus(HeroSkills.Charm, bonus);

            _heroBll.Work(_hero, _workAction);

            _hero.Charm.Should().Be(v + bonus);
        }

        #endregion Bonus for job/work

        #region Actions

        [TestMethod]
        public void ApplyAction_MoveHero()
        {
            var door = _from.Doors.First();
            var act = new ActionToMove($"Move to {door.Direction} for {door.LocationToGoId}", door.LocationToGoId);

            _heroBll.ApplyAction(_hero, act);

            _hero.CurrentLocationId.Should().Be(door.LocationToGoId);
        }

        [TestMethod]
        public void ApplyAction_ImproveCharm_Ok()
        {
            var expected = _hero.Charm + 10;
            var act = new ActionToImprove("Improve charm", HeroSkills.Charm, 4, 10);

            _heroBll.ApplyAction(_hero, act);

            _hero.Charm.Should().Be(expected);
            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero - 4);
        }

        [TestMethod]
        public void ApplyAction_RestoreMaxTtl_WhenActionToSleep()
        {
            _hero.UseTtl(MySettings.MaxTtlForHero);

            _heroBll.ApplyAction(_hero, _sleepAction);

            // Expecting that TTL will be max
            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero);
        }

        [TestMethod]
        public void ApplyAction_BuyGift_Ok()
        {
            _hero.Gifts.Clear();
            _hero.SpendMoney(-1000);

            _heroBll.ApplyAction(_hero, _buyAction);

            _hero.Gifts.Should().HaveCount(1);
        }

        #endregion Actions
    }
}
