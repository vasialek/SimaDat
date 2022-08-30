using System;

namespace SimaDat.Models.Exceptions
{
    public class NoTtlException : Exception
    {
        public NoTtlException()
        {
        }

        public NoTtlException(string msg)
            : base(msg)
        {
        }
    }
}
