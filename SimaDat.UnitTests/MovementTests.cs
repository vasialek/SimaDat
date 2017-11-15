using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class MovementTests
    {
        private ILocationBll _locationBll = null;
        private IHeroBll _heroBll = null;

        [TestInitialize]
        public void Init()
        {
            _locationBll = BllFactory.Current.LocationBll;
            _heroBll = BllFactory.Current.HeroBll;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void Test_Could_Not_Move_When_Not_In_Location()
        {
            Location from = new Location();
            Location to = new Location();
            Hero h = new Hero();
            from.LocationId = 1;
            h.CurrentLocationId = 666;

            _heroBll.MoveTo(h, from, to);
        }

        [TestMethod]
        public void Test_Move_Hero_To_Other_Location()
        {
            Location from = new Location();
            Location to = new Location();
            Hero h = new Hero();
            from.LocationId = 1;
            to.LocationId = 2;
            h.CurrentLocationId = 1;
            // Create door
            _locationBll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);

            _heroBll.MoveTo(h, from, to);

            Assert.AreEqual(to.LocationId, h.CurrentLocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotMoveException))]
        public void Test_Deny_Move_To_Location_Without_Door()
        {
            Location from = new Location();
            Location to = new Location();
            Hero h = new Hero();
            from.LocationId = 1;
            to.LocationId = 2;
            h.CurrentLocationId = 1;

            _heroBll.MoveTo(h, from, to);
        }
    }
}
