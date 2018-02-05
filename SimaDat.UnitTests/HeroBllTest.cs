using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using FluentAssertions;
using SimaDat.Models;
using SimaDat.Bll;
using SimaData.Dal;
using SimaDat.UnitTests.FakeClasses;
using SimaDat.Models.Actions;
using System.Linq;
using SimaDat.Models.Enums;

namespace SimaDat.UnitTests
{
    /// <summary>
    /// Summary description for HeroBllTest
    /// </summary>
    [TestClass]
    public class HeroBllTest
    {
        private Hero _hero = null;
        private IHeroBll _heroBll = Bll.BllFactory.Current.HeroBll;
        private ILocationBll _locationBll = new LocationBll(BllFactory.Current.CharactersBll, DalFactory.Current.LocationDal);

        private Location _from = new Location(1, "From");
        private Location _to = new Location(2, "To");

        private ActionToImprove _improveIq = null;
        private ActionToImprove _improveStrength = null;
        private ActionToImprove _improveCharm = null;
        private ActionToRest _sleepAction = null;
        private ActionToWork _workAction = null;
        private ActionToBuy _buyAction = null;

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
            _buyAction = new ActionToBuy(new Models.Items.Gift { GiftId = 1, Name = "Flower", Price = 1, FirendshipPoints = 100 });

            _locationBll.Clear();
            _locationBll.CreateDoorInLocation(_from, _to, Models.Enums.Directions.North);
            _locationBll.CreateLocation(_from);
            _locationBll.CreateLocation(_to);

            // Set Hero in from
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
            Location from = new Location();
            Location to = new Location();
            _hero.CurrentLocationId = from.LocationId;

            // Expecting exception, that movement is not possible w/o door
            _heroBll.MoveTo(_hero, from, to);
        }

        //[TestMethod]
        //[ExpectedException(typeof(NoTtlException))]
        //public void MoveTo_Exception_WhenNoTtl()
        //{
        //    // Expecting exception that Hero could not move - 0 TTL
        //    HeroProxy hero = new HeroProxy();
        //    hero.SetTtl(0);
        //    hero.CurrentLocationId = _from.LocationId;

        //    _heroBll.MoveTo(hero, _from, _to);
        //}

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void MoveTo_Exception_WhenHeroIsNotInLocationFrom()
        {
            // Set hero not in from location
            _hero.CurrentLocationId = _from.LocationId + 100;

            // Expecting exception moving from wrong location
            _heroBll.MoveTo(_hero, _from, _to);
        }

        [TestMethod]
        public void MoveTo_BadLocationIdFrom()
        {
            bool isOk = false;

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
            bool isOk = false;

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

        #endregion

        #region Movement with enter condition

        [TestMethod]
        [ExpectedException(typeof(BadConditionException))]
        public void MoveTo_EnterIsNotPossible()
        {
            // Never allow to enter
            _to.SetEnterCondition("XEP BAM", delegate(Hero h) { return false; });

            _heroBll.MoveTo(_hero, _from, _to);
        }

        [TestMethod]
        public void MoveTo_CouldNotEnterOnSaturday()
        {
            bool isOk = false;
            string violateMessage = "Closed on weekend";
            // Could not enter after Friday
            _to.SetEnterCondition(violateMessage, delegate (Hero h) { return h.Calendar.WeekDay < 6; });
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
            _to.SetEnterCondition($"You should reach next level of friendship to enter home of {girl.Name}.", (Hero h) => {
                //var owner = BllFactory.Current.LocationBll.GetOwnerOfLocation(_to.LocationId);
                return ((int)girl.FriendshipLevel >= (int)FriendshipLevels.SawHimSomewhere);
            });

            _heroBll.MoveTo(_hero, _from, _to);
        }

        #endregion

        #region Jump to

        [TestMethod]
        public void JumpTo_TtlDecreaseToOne()
        {
            int v = _hero.Ttl;

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

        #endregion

        #region Improvement

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Improve_HeroShouldHaveTtl()
        {
            HeroProxy hero = new HeroProxy();
            hero.SetTtl(0);

            // Expecting exception when no TTL to improve
            _heroBll.Improve(hero, _improveIq);
        }

        [TestMethod]
        public void Improve_TtlIsReducedAfterImprovement()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);

            _heroBll.Improve(_hero, _improveIq);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero - _improveIq.TtlToUse);
        }

        [TestMethod]
        public void Improve_IqShouldIncrease()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            int v = _hero.Iq;

            _heroBll.Improve(_hero, _improveIq);

            _hero.Iq.Should().Be(v + _improveIq.PointsToImprove);
        }

        [TestMethod]
        public void Improve_StrengthShouldIncrease()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            int v = _hero.Strength;

            _heroBll.Improve(_hero, _improveStrength);

            _hero.Strength.Should().Be(v + _improveStrength.PointsToImprove);
        }

        [TestMethod]
        [ExpectedException(typeof(NoMoneyException))]
        public void Improve_NoMoneyException_Charm()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            int v = _hero.Charm;

            _heroBll.Improve(_hero, _improveCharm);
        }

        [TestMethod]
        public void Improve_CharmShouldIncrease()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            // Add enough money
            _hero.SpendMoney(-_improveCharm.MoneyToSpent);
            int v = _hero.Charm;

            _heroBll.Improve(_hero, _improveCharm);

            _hero.Charm.Should().Be(v + _improveCharm.PointsToImprove);
        }

        [TestMethod]
        public void Improve_MoneyShouldDecrease_Charm()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            // Add enough money
            _hero.SpendMoney(-1000);
            int v = _hero.Money;

            _heroBll.Improve(_hero, _improveCharm);

            // Expecting money to be decresead on cost of charm
            _hero.Money.Should().Be(v - _improveCharm.MoneyToSpent);
        }

        #endregion

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
            int v = _hero.Calendar.Day;

            _heroBll.Sleep(_hero);

            // Should be next day after sleep
            _hero.Calendar.Day.Should().BeGreaterThan(v);
        }

        #endregion

        #region Work

        [TestMethod]
        public void Work_DescreaseTtl()
        {
            int v = _hero.Ttl;

            _heroBll.Work(_hero, _workAction);

            _hero.Ttl.Should().Be(v - _workAction.TtlToUse);
        }

        [TestMethod]
        public void Work_EarnMoney()
        {
            int v = _hero.Money;

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

        #endregion


        #region Penalty for work/job

        [TestMethod]
        public void Work_DecreaseIq_WhenPenaltyIs()
        {
            _hero.ModifySkill(HeroSkills.Iq, 100);
            int v = _hero.Iq;
            int penalty = 1;
            _workAction.SetPenalty(HeroSkills.Iq, penalty);

            _heroBll.Work(_hero, _workAction);

            // Expecting IQ to decrease by penalty
            _hero.Iq.Should().Be(v - penalty);
        }

        [TestMethod]
        public void Work_NoDecreaseIq_WhenIqIsZerro()
        {
            int penalty = 1000;
            _workAction.SetPenalty(HeroSkills.Iq, penalty);

            _heroBll.Work(_hero, _workAction);

            // Do not decrease IQ below 0
            _hero.Iq.Should().BeGreaterOrEqualTo(0);
        }

        #endregion

        #region Bonus for job/work

        [TestMethod]
        public void Work_IncreaseCharm_WhenBonusIs()
        {
            int bonus = 1;
            int v = _hero.Charm;
            _workAction.SetBonus(HeroSkills.Charm, bonus);

            _heroBll.Work(_hero, _workAction);

            _hero.Charm.Should().Be(v + bonus);
        }

        #endregion

        #region Actions

        [TestMethod]
        public void ApplyAction_MoveHero()
        {
            Location.Door d = _from.Doors.First();
            var a = new ActionToMove($"Move to {d.Direction} for {d.LocationToGoId}", d.LocationToGoId);

            _heroBll.ApplyAction(_hero, a);

            _hero.CurrentLocationId.Should().Be(d.LocationToGoId);
        }

        [TestMethod]
        public void ApplyAction_ImproveCharm_Ok()
        {
            var a = new ActionToImprove("Improve charm", HeroSkills.Charm, 4, 10);

            _heroBll.ApplyAction(_hero, a);
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

        #endregion
    }
}
