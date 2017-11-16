using AvUtils;
using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Console
{
    internal class SdConsole
    {
        private ILocationBll _locationBll = null;
        private Dumper _dumper = new Dumper();

        public SdConsole(ILocationBll locationBll)
        {
            if (locationBll == null)
            {
                throw new ArgumentNullException(nameof(locationBll));
            }

            _locationBll = locationBll;
        }

        public void DisplayLocations()
        {
            var locations = _locationBll.GetAllLocations();
            if (locations?.Count > 0)
            {
                foreach (var l in locations)
                {
                    Output.WriteLine(_dumper.Dump(l, $"Location {l.Name}"));
                    if (l.Doors?.Count > 0)
                    {
                        Output.WriteLine(_dumper.Dump(l.Doors.ToArray()));
                    }
                }
            }
            else
            {
                Output.WriteLine(ConsoleColor.Red, "No locations to display");
            }
        }
    }
}
