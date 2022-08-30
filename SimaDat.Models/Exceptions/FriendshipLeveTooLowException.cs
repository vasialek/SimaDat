using System;

namespace SimaDat.Models.Exceptions
{
    public class FriendshipLeveTooLowException : Exception
    {
        public FriendshipLeveTooLowException(string msg)
            : base(msg)
        {
        }
    }
}
