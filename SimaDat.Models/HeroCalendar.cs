using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models
{
    public class HeroCalendar
    {
        /// <summary>
        /// Monday = 1
        /// </summary>
        private int _weekDay = 1;

        public int Day { get; private set; }

        public string WeekDay
        {
            get
            {
                if (_weekDay > 7)
                {
                    _weekDay = 1;
                }

                switch (_weekDay)
                {
                    case 1:
                        return "Mon";
                    case 2:
                        return "Tue";
                    case 3:
                        return "Wed";
                    case 4:
                        return "Thu";
                    case 5:
                        return "Fri";
                    case 6:
                        return "Sat";
                    case 7:
                        return "Sun";
                }
                throw new ArgumentOutOfRangeException("Week day is incorrect: " + _weekDay);
            }
        }

        public HeroCalendar()
            : this(1)
        {
            Day = 1;
        }

        public HeroCalendar(int weekDay)
        {
            _weekDay = weekDay;
        }
    }
}
