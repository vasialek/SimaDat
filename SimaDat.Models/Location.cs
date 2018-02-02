using System;
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

        public int OwnerId { get; set; } = -1;

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

        public string ViolateEnterMessage { get; private set; } = "Hero is not allowed to enter.";
        private Func<Characters.Hero, bool> _couldEnterAction = (Characters.Hero h) => { return true; };

        public bool CouldEnter(Characters.Hero hero)
        {
            return _couldEnterAction(hero);
        }

        public void SetEnterCondition(Func<Characters.Hero, bool> f)
        {
            SetEnterCondition(null, f);
        }

        public void SetEnterCondition(string violateMsg, Func<Characters.Hero, bool> f)
        {
            ViolateEnterMessage = violateMsg;
            _couldEnterAction = f;
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
