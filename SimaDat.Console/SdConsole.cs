﻿using AvUtils;
using SimaDat.Models.Actions;
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
        private ICharactersBll _charsBll = null;
        private Dumper _dumper = new Dumper();

        public SdConsole(Hero hero, ILocationBll locationBll, ICharactersBll charsBll)
        {
            if (hero == null)
            {
                throw new ArgumentNullException(nameof(hero));
            }
            if (locationBll == null)
            {
                throw new ArgumentNullException(nameof(locationBll));
            }
            if (charsBll == null)
            {
                throw new ArgumentNullException(nameof(charsBll));
            }

            _hero = hero;
            _locationBll = locationBll;
            _charsBll = charsBll;
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

        public void DisplayGirls()
        {
            var girls = _charsBll.GetAll();
            for (int i = 0; i < girls?.Count; i++)
            {
                Output.WriteLine(_dumper.Dump(girls[i]));
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

        internal void HeroMenu()
        {
            try
            {
                bool isRunning = true;

                do
                {
                    Output.Clear();
                    this.DisplayHeroStats();

                    var currentLocation = _locationBll.GetLocationById(_hero.CurrentLocationId);
                    Output.WriteLine("You are in {0}", currentLocation.Name);
                    var actions = _locationBll.GetPossibleActions(currentLocation);
                    var skills = _locationBll.GetSkillsToImprove(currentLocation);

                    var menu = new Menu();

                    menu.Add("Back to main menu", () => { isRunning = false; }, ConsoleColor.DarkYellow);

                    if (actions?.Count > 0)
                    {
                        foreach (var a in actions)
                        {
                            menu.Add(a.Name, () => { DoAction(a); });
                        } 
                    }
                    if (skills?.Count > 0)
                    {
                        foreach (var s in skills)
                        {
                            menu.Add($"+{s.ImprovementPoints} to {s.Skill}, takes {s.TtlToUse} hours", () =>
                            {
                                DoAction(new ActionToImprove(s.Skill.ToString(), s.Skill, s.TtlToUse, s.ImprovementPoints));
                            });
                        } 
                    }

                    menu.Display();

                } while (isRunning);
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, ex.Message);
                Output.WriteLine(ex.ToString());
            }
        }

        protected void DoAction(ActionToDo action)
        {
            Output.WriteLine("Performing {0}", action.Name);
            Bll.BllFactory.Current.HeroBll.ApplyAction(_hero, action);
            Input.ReadKey();
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
                    var improvementsAvailable = _locationBll.GetSkillsToImprove(currentLocation);

                    var menu = new Menu();

                    menu.Add("Back to main menu", () => { isRunning = false; }, ConsoleColor.DarkYellow);
                    if (improvementsAvailable?.Count > 0)
                    {
                        foreach (var improve in improvementsAvailable)
                        {
                            menu.Add($"Improve your {improve.Skill} using {improve.TtlToUse} hours", () => { Output.WriteLine(ConsoleColor.Green, "You have improved {0}", improve); });
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

        internal void DisplayHeroStats()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ".PadLeft(80, ' '));
            sb.AppendFormat("|{0,8}|{1,8}|{2,8}|{3,12}|{4,8}|", "TTL", "IQ", "Charm", "Strength", "Money").AppendLine();
            sb.Append(" ".PadLeft(80, ' '));
            sb.AppendFormat("|{0,8}|{1,8}|{2,8}|{3,12}|{4,8}|", _hero.Ttl, _hero.Iq, _hero.Charm, _hero.Strength, 0);

            Output.WriteLine(ConsoleColor.DarkGreen, sb.ToString());
        }
    }
}
