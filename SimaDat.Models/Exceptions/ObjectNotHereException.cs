using System;

namespace SimaDat.Models.Exceptions
{
    public class ObjectNotHereException : Exception
    {
        public ObjectNotHereException(string message)
            : base(message)
        {
        }
    }
}
