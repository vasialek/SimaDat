using System;

namespace SimaDat.Models.Exceptions
{
    public class DirectionInUseException : Exception
    {
        public DirectionInUseException()
            : base()
        {
        }

        public DirectionInUseException(string message)
            : base(message)
        {
        }
    }
}
