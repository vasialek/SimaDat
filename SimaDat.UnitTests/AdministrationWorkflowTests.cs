using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaData.Dal;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Interfaces;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class AdministrationWorkflowTests
    {
        private Hero _hero = null;
        private ILocationBll _locationBll = null;
        private IHeroBll _heroBll = null;

        private Location _home = null;
        private Location _square = null;
        private Location _school = null;
        private Location _pub = null;
        private Location _gym = null;

        [TestInitialize]
        public void Init()
        {
            // TODO: Replace this with fake after real DB is conected
            _locationBll = new LocationBll(new LocationDal());
            _heroBll = new HeroBll(_locationBll);
            _hero = new Hero();
            _hero.ResetTtl();

            /*
            *       Hope to get such map
            *               PUB
            *                 |
            *   SCHOOL <->  SQUARE  <-> GYM
            *                 |
            *               HOME
            */

            _home = new Location(1, "Home");
            _square = new Location(2, "Square");
            _school = new Location(3, "School");
            _pub = new Location(4, "Pub");
            _gym = new Location(5, "Gym");
        }

        [TestMethod]
        public void Test_Walk_Simple_Map()
        {
            _locationBll.CreateDoorInLocation(_home, _square, Models.Enums.Directions.North);
            _locationBll.CreateDoorInLocation(_square, _school, Models.Enums.Directions.West);
            _locationBll.CreateDoorInLocation(_square, _pub, Models.Enums.Directions.North);
            _locationBll.CreateDoorInLocation(_square, _gym, Models.Enums.Directions.East);

            _hero.CurrentLocationId = _home.LocationId;

            _heroBll.MoveTo(_hero, _home, _square);
            Assert.AreEqual(_square.LocationId, _hero.CurrentLocationId);

            _heroBll.MoveTo(_hero, _square, _school);
            Assert.AreEqual(_school.LocationId, _hero.CurrentLocationId);

            _heroBll.MoveTo(_hero, _school, _square);
            _heroBll.MoveTo(_hero, _square, _gym);
            Assert.AreEqual(_gym.LocationId, _hero.CurrentLocationId);

            _heroBll.MoveTo(_hero, _gym, _square);
            _heroBll.MoveTo(_hero, _square, _pub);
            Assert.AreEqual(_pub.LocationId, _hero.CurrentLocationId);
        }
    }
}
