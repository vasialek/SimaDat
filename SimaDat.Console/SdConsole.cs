using AvUtils;
using SimaDat.Models.Characters;
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
        private Hero _hero = null;
        private ILocationBll _locationBll = null;
        private Dumper _dumper = new Dumper();

        public SdConsole(Hero hero, ILocationBll locationBll)
        {
            if (hero == null)
            {
                throw new ArgumentNullException(nameof(hero));
            }
            if (locationBll == null)
            {
                throw new ArgumentNullException(nameof(locationBll));
            }

            _hero = hero;
            _locationBll = locationBll;
        }

        public void DisplayHero()
        {
            Output.WriteLine(ConsoleColor.Green, _dumper.Dump(_hero, "Hero"));
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

        internal void MoveHero()
        {
            bool isRunning = true;

            try
            {
                while (isRunning)
                {
                    var currentLocation = _locationBll.GetLocationById(_hero.CurrentLocationId);
                    Output.WriteLine(ConsoleColor.Green, "You are at Location {0} (#{1})", currentLocation.Name, _hero.CurrentLocationId);

                    var menu = new Menu();

                    menu.Add("Back to main menu", () => { isRunning = false; }, ConsoleColor.DarkYellow);
                    foreach (var d in currentLocation.Doors)
                    {
                        menu.Add($"Move to {d.Direction} to location #{d.LocationToGoId}", () => {
                            var locationToGo = _locationBll.GetLocationById(d.LocationToGoId);
                            Bll.BllFactory.Current.HeroBll.MoveTo(_hero, currentLocation, locationToGo);
                        });
                    }

                    menu.Display();
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, "Error moving Hero. " + ex.Message);
                Output.WriteLine(ex.ToString());
            }
        }

        internal void ImproveHero()
        {
            bool isRunning = true;

            try
            {
                while (isRunning)
                {
                    var currentLocation = _locationBll.GetLocationById(_hero.CurrentLocationId);
                    Output.WriteLine(ConsoleColor.Green, "You are at Location {0} (#{1})", currentLocation.Name, _hero.CurrentLocationId);
                    var improvementsAvailable = _locationBll.GetAbilitiesToImprove(currentLocation);

                    var menu = new Menu();

                    menu.Add("Back to main menu", () => { isRunning = false; }, ConsoleColor.DarkYellow);
                    if (improvementsAvailable?.Count > 0)
                    {
                        foreach (var improve in improvementsAvailable)
                        {
                            menu.Add($"Improve your {improve}", () => { Output.WriteLine(ConsoleColor.Green, "You have improved {0}", improve); });
                        }
                    }
                    else
                    {
                        Output.WriteLine(ConsoleColor.Red, "No improvments are available her");
                    }

                    menu.Display();
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, "Error moving Hero. " + ex.Message);
                Output.WriteLine(ex.ToString());
            }
        }
    }
}
