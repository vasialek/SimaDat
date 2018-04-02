using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
