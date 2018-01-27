using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Exceptions
{
    public class BadConditionException : Exception
    {
        public BadConditionException()
            : base()
        {
        }

        public BadConditionException(string msg)
            : base(msg)
        {
        }
    }
}
