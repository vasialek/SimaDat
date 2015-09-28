using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Exceptions;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class LocationBllTest
    {
        private ILocationBll _bll = new LocationBll();

        [TestMethod]
        public void Test_CouldNot_Move_At_All()
        {
            // No doors in location
            Location from = new Location();
            Location to = new Location();

            bool couldMove = _bll.CouldMoveTo(from, to);

            Assert.IsFalse(couldMove);
        }

        [TestMethod]
        public void Test_Could_Move_Everywhere()
        {
            Location from = new Location();
            Location to = new Location();

            bool couldMove = _bll.CouldMoveTo(from, to);

        }

        [TestMethod]
        public void Test_Create_First_Door()
        {
            Location from = new Location();
            Location to = new Location();

            bool couldMove = _bll.CouldMoveTo(from, to);

            // No door created
            Assert.IsFalse(couldMove);

            _bll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);

            // Door is created
            couldMove = _bll.CouldMoveTo(from, to);
            Assert.IsTrue(couldMove);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectionInUseException))]
        public void Test_Deny_Create_Same_Door()
        {
            Location from = new Location();
            Location to = new Location();

            _bll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);
            _bll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);
        }
    }
}
