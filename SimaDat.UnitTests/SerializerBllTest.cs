using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class SerializerBllTest
    {
        private ISerializer _serializer;
        private Location _locationHome;
        private Location _locationPub;

        [TestInitialize]
        public void TestInit()
        {
            _serializer = new SerializerBll();

            _locationPub = new Location(555, "Pub");
            _locationHome = new Location(100, "Home");
            _locationHome.Doors = new List<Location.Door> { new Location.Door { Direction = Models.Enums.Directions.North, LocationToGoId = _locationPub.LocationId } };
        }

        #region Deserialize Location

        [TestMethod]
        public void DeserializeLocation_NotNull()
        {
            var json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var location = _serializer.DeserializeLocation(json);

            location.Should().NotBeNull();
        }

        [TestMethod]
        public void DeserializeLocation_CheckName()
        {
            var json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var location = _serializer.DeserializeLocation(json);

            location.Name.Should().Be("Home");
        }

        [TestMethod]
        public void DeserializeLocation_CheckDoorsNotNull()
        {
            var json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var location = _serializer.DeserializeLocation(json);

            location.Doors.Should().NotBeNull();
        }

        [TestMethod]
        public void DeserializeLocation_CheckDirection_ForDoor()
        {
            var json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"SE\",\"555\"]]}";

            var location = _serializer.DeserializeLocation(json);

            location.Doors.Single().Direction.Should().Be(Models.Enums.Directions.SouthEast);
        }

        [TestMethod]
        public void DeserializeLocation_CheckLocationToGoId_ForDoor()
        {
            var json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var location = _serializer.DeserializeLocation(json);

            location.Doors.Single().LocationToGoId.Should().Be(555);
        }

        #endregion Deserialize Location

        #region Serialize Location

        [TestMethod]
        public void Serialize_NotNull_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            actual.Should().NotBeNull();
        }

        [TestMethod]
        public void Serialize_CheckName_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            actual.Should().Contain($"\"{_locationHome.Name}\"");
        }

        [TestMethod]
        public void Serialize_CheckLocationId_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            actual.Should().Contain($"\"id\":{_locationHome.LocationId}");
        }

        [TestMethod]
        public void Serialize_CheckDoors_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            actual.Should().Contain("\"doors\":");
        }

        [TestMethod]
        public void Serialize_CheckDoorDirection_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            var z = _locationHome.Doors.Select(x => new string[] { x.Direction.ToString(), x.LocationToGoId.ToString() }).ToList();
            actual.Should().Contain("[\"N\"");
        }

        [TestMethod]
        public void Serialize_CheckDoorLocationToGoId_ForLocation()
        {
            var actual = _serializer.Serialize(_locationHome);

            var z = _locationHome.Doors.Select(x => new string[] { x.Direction.ToString(), x.LocationToGoId.ToString() }).ToList();
            actual.Should().Contain(",\"555\"]");
        }

        #endregion Serialize Location

        #region Serialize Door

        [TestMethod]
        public void Serialize_CheckDirection_ForDoor()
        {
            var door = new Location.Door { Direction = Models.Enums.Directions.NorthWest, LocationToGoId = 666 };

            var actual = _serializer.Serialize(door);

            actual.Should().Contain("\"NW\"");
        }

        [TestMethod]
        public void Serialize_CheckLocationId_ForDoor()
        {
            var door = new Location.Door { Direction = Models.Enums.Directions.NorthWest, LocationToGoId = 666 };

            var actual = _serializer.Serialize(door);

            actual.Should().Contain($"\"{door.LocationToGoId}\"");
        }

        #endregion Serialize Door
    }
}
