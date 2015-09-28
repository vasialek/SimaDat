using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
