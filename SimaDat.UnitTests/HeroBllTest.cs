using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Characters;
using SimaDat.Models.Skills;
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
        private ILocationBll _locationBll = new LocationBll(DalFactory.Current.LocationDal);

        private Location _from = new Location(1, "From");
        private Location _to = new Location(2, "To");

        private SkillImprovement _improveIq = null;
        private SkillImprovement _improveStrength = null;
        private SkillImprovement _improveCharm = null;
        private ActionToRest _sleepAction = null;

        [TestInitialize]
        public void TestInit()
        {
            _hero = new Hero
            {
                Name = "Test hero"
            };
            _hero.ResetTtl();

            _improveIq = new SkillImprovement { Skill = Models.Enums.HeroSkills.Iq, ImprovementPoints = 1, TtlToUse = 4 };
            _improveStrength = new SkillImprovement { Skill = HeroSkills.Strength, ImprovementPoints = 2, TtlToUse = 5 };
            _improveCharm = new SkillImprovement { Skill = HeroSkills.Charm, ImprovementPoints = 3, TtlToUse = 6 };
            _sleepAction = new ActionToRest();

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

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void MoveTo_Exception_WhenNoTtl()
        {
            // Expecting exception that Hero could not move - 0 TTL
            HeroProxy hero = new HeroProxy();
            hero.SetTtl(0);
            hero.CurrentLocationId = _from.LocationId;

            _heroBll.MoveTo(hero, _from, _to);
        }

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

            _hero.Iq.Should().Be(v + _improveIq.ImprovementPoints);
        }

        [TestMethod]
        public void Improve_StrengthShouldIncrease()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            int v = _hero.Strength;

            _heroBll.Improve(_hero, _improveStrength);

            _hero.Strength.Should().Be(v + _improveStrength.ImprovementPoints);
        }

        [TestMethod]
        public void Improve_CharmShouldIncrease()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);
            int v = _hero.Charm;

            _heroBll.Improve(_hero, _improveCharm);

            _hero.Charm.Should().Be(v + _improveCharm.ImprovementPoints);
        }

        #endregion

        #region Sleep

        [TestMethod]
        public void Sleep_ShouldRestoreMaxTtl()
        {
            _heroBll.Sleep(_hero);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero);
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

        #endregion
    }
}
