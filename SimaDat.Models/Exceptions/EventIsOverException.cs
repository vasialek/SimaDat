using System;

namespace SimaDat.Models.Exceptions
{
    public class EventIsOverException : Exception
    {
        public EventIsOverException()
            : this(null)
        {
        }

        public EventIsOverException(string msg)
            : base(msg)
        {
        }
    }
}
