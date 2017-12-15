using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
