using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Exceptions;
using SimaData.Dal;
using SimaDat.Models.Enums;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class LocationBllTest
    {
        private ILocationBll _bll = new LocationBll(DalFactory.Current.LocationDal);

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
            Assert.IsFalse(couldMove);
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

            // Test door is both way
            couldMove = _bll.CouldMoveTo(to, from);
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

        # region Create and modify locations

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Deny_Creation_Of_Null_Location()
        {
            _bll.CreateLocation(null);
        }

        [TestMethod]
        public void Test_Create_First_Location_Ok()
        {
            var location = new Location();
            _bll.CreateLocation(location);

            var locations = _bll.GetAllLocations();

            Assert.AreEqual(1, locations.Count);
        }

        #endregion

        [TestMethod]
        public void Test_Opposite_Direction()
        {
            Directions d, od;

            d = Directions.North;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.South, od);

            d = Directions.South;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.North, od);

            d = Directions.East;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.West, od);

            d = Directions.West;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.East, od);

            d = Directions.NorthEast;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.SouthWest, od);

            d = Directions.NorthWest;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.SouthEast, od);

            d = Directions.SouthEast;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.NorthWest, od);

            d = Directions.SouthWest;
            od = _bll.GetOppositeDirection(d);
            Assert.AreEqual(Directions.NorthEast, od);
        }
    }
}
