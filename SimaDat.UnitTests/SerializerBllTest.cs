using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models;
using System.Collections.Generic;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using FluentAssertions;
using System.Linq;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class SerializerBllTest
    {
        private ISerializer _serializer = null;
        private Location _locationHome = null;
        private Location _locationPub = null;

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
            string json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var loc = _serializer.DeserializeLocation(json);

            loc.Should().NotBeNull();
        }

        [TestMethod]
        public void DeserializeLocation_CheckName()
        {
            string json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var loc = _serializer.DeserializeLocation(json);

            loc.Name.Should().Be("Home");
        }
        [TestMethod]
        public void DeserializeLocation_CheckDoorsNotNull()
        {
            string json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var loc = _serializer.DeserializeLocation(json);

            loc.Doors.Should().NotBeNull();
        }

        [TestMethod]
        public void DeserializeLocation_CheckDirection_ForDoor()
        {
            string json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"SE\",\"555\"]]}";

            var loc = _serializer.DeserializeLocation(json);

            loc.Doors.Single().Direction.Should().Be(Models.Enums.Directions.SouthEast);
        }

        [TestMethod]
        public void DeserializeLocation_CheckLocationToGoId_ForDoor()
        {
            string json = "{\"name\":\"Home\",\"id\":100,\"doors\":[[\"N\",\"555\"]]}";

            var loc = _serializer.DeserializeLocation(json);

            loc.Doors.Single().LocationToGoId.Should().Be(555);
        }

        #endregion

        #region Serialize Location

        [TestMethod]
        public void Serialize_NotNull_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            s.Should().NotBeNull();
        }

        [TestMethod]
        public void Serialize_CheckName_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            s.Should().Contain($"\"{_locationHome.Name}\"");
        }

        [TestMethod]
        public void Serialize_CheckLocationId_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            s.Should().Contain($"\"id\":{_locationHome.LocationId}");
        }

        [TestMethod]
        public void Serialize_CheckDoors_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            s.Should().Contain("\"doors\":");
        }

        [TestMethod]
        public void Serialize_CheckDoorDirection_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            var z =_locationHome.Doors.Select(x => new string[] { x.Direction.ToString(), x.LocationToGoId.ToString() }).ToList();
            s.Should().Contain("[\"N\"");
        }

        [TestMethod]
        public void Serialize_CheckDoorLocationToGoId_ForLocation()
        {
            string s = _serializer.Serialize(_locationHome);

            var z = _locationHome.Doors.Select(x => new string[] { x.Direction.ToString(), x.LocationToGoId.ToString() }).ToList();
            s.Should().Contain(",\"555\"]");
        }

        #endregion

        #region Serialize Door

        [TestMethod]
        public void Serialize_CheckDirection_ForDoor()
        {
            Location.Door door = new Location.Door { Direction = Models.Enums.Directions.NorthWest, LocationToGoId = 666 };

            string s = _serializer.Serialize(door);

            s.Should().Contain("\"NW\"");
        }

        [TestMethod]
        public void Serialize_CheckLocationId_ForDoor()
        {
            Location.Door door = new Location.Door { Direction = Models.Enums.Directions.NorthWest, LocationToGoId = 666 };

            string s = _serializer.Serialize(door);

            s.Should().Contain($"\"{door.LocationToGoId}\"");
        }

        #endregion
    }
}
