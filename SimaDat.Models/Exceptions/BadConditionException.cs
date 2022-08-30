using System;

namespace SimaDat.Models.Exceptions
{
    public class BadConditionException : Exception
    {
        public BadConditionException()
            : base()
        {
        }

        public BadConditionException(string msg)
            : base(msg)
        {
        }
    }
}
