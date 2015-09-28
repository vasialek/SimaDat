using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
