using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Exceptions
{
    public class FriendshipLeveTooLowException : Exception
    {
        public FriendshipLeveTooLowException(string msg)
            : base(msg)
        {
        }
    }
}
