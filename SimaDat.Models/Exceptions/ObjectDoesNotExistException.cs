using System;

namespace SimaDat.Models.Exceptions
{
    public class ObjectDoesNotExistException : Exception
    {
        public int ObjectId { get; }

        public ObjectDoesNotExistException(string msg, int objectId)
            : base(msg)
        {
            ObjectId = objectId;
        }
    }
}
