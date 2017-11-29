using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Exceptions
{
    public class ObjectDoesNotExistException : Exception
    {
        public int ObjectId { get; private set; }

        public ObjectDoesNotExistException(string msg, int objectId)
            : base(msg)
        {
            ObjectId = objectId;
        }
    }
}
