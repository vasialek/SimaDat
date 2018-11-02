using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Actions
{
    public class ActionToQuit : ActionToDo
    {
        public ActionToQuit(string name)
            : base(name, 0)
        {
        }
    }
}
