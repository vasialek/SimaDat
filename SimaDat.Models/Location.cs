﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Enums;

namespace SimaDat.Models
{
    public class Location
    {

        public class Door
        {
            public int LocationToGoId { get; set; }
            public Directions Direction { get; set; }
        }

        public int LocationId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Where user could go
        /// </summary>
        public IList<Door> Doors { get; set; }

        public Location()
            : this(-1, String.Empty)
        {
        }

        public Location(string name)
            : this(-1, name)
        {
        }

        public Location(int locationId, string name)
        {
            LocationId = locationId;
            Name = name;
            Doors = new List<Door>();
        }

        public Door GetDoorAtDirection(Directions d)
        {
            if (Doors == null)
            {
                return null;
            }
            return Doors.FirstOrDefault(x => x.Direction == d);
        }
    }
}