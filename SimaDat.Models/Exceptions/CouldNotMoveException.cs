using System;

namespace SimaDat.Models.Exceptions
{
    public class CouldNotMoveException : Exception
    {
        public CouldNotMoveException(string message)
            : base(message)
        {
        }
    }
}
