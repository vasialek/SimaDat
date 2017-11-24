using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Exceptions
{
    /// <summary>
    /// Indicates that Hero could not improve skill or any other action because he needs to rest
    /// </summary>
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
