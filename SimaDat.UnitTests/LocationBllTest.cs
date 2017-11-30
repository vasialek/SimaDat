using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Exceptions;
using SimaData.Dal;
using SimaDat.Models.Enums;
using SimaDat.Models.Interfaces;
using FluentAssertions;
using System.Linq;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class LocationBllTest
    {
        private ILocationBll _bll = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new LocationBll(DalFactory.Current.LocationDal);
        }

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
        public void CouldMoveTo_NoWay()
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
        public void CreateDoorInLocation_DenySameDoor()
        {
            Location from = new Location();
            Location to = new Location();

            _bll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);
            _bll.CreateDoorInLocation(from, to, Models.Enums.Directions.North);
        }

        # region Create and modify locations

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateLocation_DenyNullLocation()
        {
            _bll.CreateLocation(null);
        }

        [TestMethod]
        public void CreateLocation_FirstLocation()
        {
            _bll.Clear();
            _bll.CreateLocation(new Location());

            var locations = _bll.GetAllLocations();

            Assert.AreEqual(1, locations.Count);
        }

        [TestMethod]
        public void CreateLocation_DenySameLocationName()
        {
            bool isOk = false;
            _bll.Clear();

            _bll.CreateLocation(new Location("SameLocation"));

            try
            {
                // Expecting exception on duplicate name
                _bll.CreateLocation(new Location("SameLocation"));
            }
            catch (ArgumentException aex) when (aex.ParamName == "Name")
            {
                isOk = true;
            }

            isOk.Should().BeTrue("expecting exception when location name is not unique.");
        }

        #endregion

        #region Actions tests

        [TestMethod]
        public void GetPossibleActions_GetOneMove()
        {
            Location from = new Location();
            Location to = new Location();
            _bll.CreateDoorInLocation(from, to, Directions.North);

            var actions = _bll.GetPossibleActions(from);

            // Should be just 1 action - to move
            actions.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetPossibleActions_GetMoveToTtl0()
        {
            Location from = new Location();
            Location to = new Location();
            _bll.CreateDoorInLocation(from, to, Directions.North);

            var actions = _bll.GetPossibleActions(from);

            actions.Single().TtlToUse.Should().Be(0);
        }

        #endregion

        #region Maintain

        [TestMethod]
        public void Clear_Ok()
        {
            _bll.CreateLocation(new Location());

            _bll.Clear();

            _bll.GetAllLocations().Should().HaveCount(0);
        }

        #endregion

        [TestMethod]
        public void GetOppositeDirection_Ok()
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
