using System;

namespace SimaDat.Models.Exceptions
{
    public class NoMoneyException : Exception
    {
        public NoMoneyException()
        {
        }

        public NoMoneyException(string msg)
            : base(msg)
        {
        }
    }
}
