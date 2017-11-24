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

        private Location _from = new Location();
        private Location _to = new Location();

        private SkillImprovement _improveIq = null;

        [TestInitialize]
        public void TestInit()
        {
            _hero = new Hero
            {
                Name = "Test hero"
            };
            _hero.ResetTtl();

            _improveIq = new SkillImprovement { Skill = Models.Enums.HeroSkills.Iq, ImprovementPoints = 1, TtlToUse = 4 };

            _from.LocationId = 1;
            _to.LocationId = 2;
            _locationBll.CreateDoorInLocation(_from, _to, Models.Enums.Directions.North);

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

        #endregion
        
        #region Sleep

        [TestMethod]
        public void Sleep_ShouldRestoreMaxTtl()
        {
            _heroBll.Sleep(_hero);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero);
        }

        #endregion
    }
}
