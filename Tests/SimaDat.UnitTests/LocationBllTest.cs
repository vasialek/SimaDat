using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Core;
using SimaDat.Models;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaData.Dal;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class LocationBllTest
    {
        private ILocationBll _bll;
        private Location _from;
        private Location _to;

        [TestInitialize]
        public void TestInit()
        {
            _from = new Location("From");
            _to = new Location("To");

            _bll = new LocationBll(BllFactory.Current.CharactersBll, DalFactory.Current.LocationDal);
            _bll.Clear();
            _bll.CreateLocation(_from);
            _bll.CreateLocation(_to);
            _bll.CreateDoorInLocation(_from, _to, Directions.North);
        }

        [TestMethod]
        public void CouldMoveTo_NoWay()
        {
            var from = new Location();
            var to = new Location();

            var couldMove = _bll.CouldMoveTo(from, to);
            Assert.IsFalse(couldMove);
        }

        [TestMethod]
        public void Test_Create_First_Door()
        {
            var from = new Location();
            var to = new Location();

            var actual = _bll.CouldMoveTo(from, to);

            // No door created
            Assert.IsFalse(actual);

            _bll.CreateDoorInLocation(from, to, Directions.North);

            // Door is created
            actual = _bll.CouldMoveTo(from, to);
            Assert.IsTrue(actual);

            // Test door is both way
            actual = _bll.CouldMoveTo(to, from);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectionInUseException))]
        public void CreateDoorInLocation_DenySameDoor()
        {
            var from = new Location();
            var to = new Location();

            _bll.CreateDoorInLocation(from, to, Directions.North);
            _bll.CreateDoorInLocation(from, to, Directions.North);
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
            var isOk = false;
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
            var actions = _bll.GetPossibleActions(_from);

            // Should be just 1 action - to move
            actions.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetPossibleActions_GetMoveToTtl0()
        {
            var actions = _bll.GetPossibleActions(_from);

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
