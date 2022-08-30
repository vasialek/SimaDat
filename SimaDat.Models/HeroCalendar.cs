using System;

namespace SimaDat.Models
{
    public class HeroCalendar
    {
        public int WeekDay { get; private set; }

        public int Day { get; private set; }

        public string WeekDayShort
        {
            get
            {
                if (WeekDay > 7)
                {
                    WeekDay = 1;
                }

                switch (WeekDay)
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
                throw new ArgumentOutOfRangeException("Week day is incorrect: " + WeekDay);
            }
        }

        public HeroCalendar()
            : this(1)
        {
            Day = 1;
        }

        public HeroCalendar(int weekDay)
        {
            WeekDay = weekDay;
        }

        public void NextDay()
        {
            Day++;
            WeekDay++;
            if (WeekDay > 7)
            {
                WeekDay = 1;
            }
        }
    }
}
