using AvUtils;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace SimaDatConsole
{
    internal class SdConsole
    {
        private readonly Hero _hero;
        private readonly ILocationBll _locationBll;
        private readonly ICharactersBll _charactersBll;
        private readonly IHeroBll _heroBll;
        private readonly Dumper _dumper = new Dumper();

        public SdConsole(Hero hero, ILocationBll locationBll, ICharactersBll charsBll)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _locationBll = locationBll ?? throw new ArgumentNullException(nameof(locationBll));
            _charactersBll = charsBll ?? throw new ArgumentNullException(nameof(charsBll));
            _heroBll = SimaDat.Bll.BllFactory.Current.HeroBll;
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
            var girls = _charactersBll.GetAll();
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
                        menu.Add($"Move to {d.Direction} to location #{d.LocationToGoId}", () =>
                        {
                            var locationToGo = _locationBll.GetLocationById(d.LocationToGoId);
                            _heroBll.MoveTo(_hero, currentLocation, locationToGo);
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
            bool isRunning = true;
            string error = null;
            string msg = null;
            do
            {
                try
                {
                    var locations = _locationBll.GetAllLocations();

                    Output.Clear();
                    this.DisplayHeroStats();

                    if (String.IsNullOrEmpty(error) == false)
                    {
                        Output.WriteLine(ConsoleColor.Red, error);
                        error = null;
                    }
                    if (String.IsNullOrEmpty(msg) == false)
                    {
                        Output.WriteLine(ConsoleColor.Green, msg);
                        msg = null;
                    }

                    var currentLocation = _locationBll.GetLocationById(_hero.CurrentLocationId);
                    Output.WriteLine("You are in {0}", currentLocation.Name);
                    var actions = _locationBll.GetPossibleActions(currentLocation);
                    var skills = _locationBll.GetSkillsToImprove(currentLocation);
                    var girls = _charactersBll.FindInLocation(_hero.CurrentLocationId);

                    var menu = new Menu();

                    menu.Add("Back to main menu", () => { isRunning = false; }, ConsoleColor.DarkYellow);
                    if (_hero.HasJumper)
                    {
                        menu.Add("Use jumper", () => { JumpTo(); }, ConsoleColor.Green);
                    }

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
                            menu.Add(s.ShortDescription, () =>
                            {
                                DoAction(s);
                            });
                        }
                    }
                    if (girls?.Count > 0)
                    {
                        foreach (var g in girls)
                        {
                            menu.Add($"Girl {g.Name}, relations {g.FriendshipLevel}", () => { });
                            menu.Add("    Say 'Hi'", () => { _charactersBll.SayHi(_hero, g); });
                            menu.Add("    Talk wit her", () => { _charactersBll.Talk(_hero, g); });
                            menu.Add($"    Present {GiftTypes.Flower}", () => { _charactersBll.Present(_hero, g, GiftTypes.Flower); });
                            menu.Add($"    Present {GiftTypes.TeddyBear}", () => { _charactersBll.Present(_hero, g, GiftTypes.TeddyBear); });
                            menu.Add($"    Present {GiftTypes.DiamondRing}", () => { _charactersBll.Present(_hero, g, GiftTypes.DiamondRing); });
                            menu.Add("    Ask dating", () =>
                            {
                                if (_charactersBll.AskDating(_hero, g))
                                {
                                    msg = "You are dating";
                                }
                                else
                                {
                                    error = "She rejects";
                                }
                            });
                        }
                    }

                    menu.Display();
                }
                catch (BadConditionException bcex)
                {
                    //Output.WriteLine(ConsoleColor.Red, bcex.Message);
                    error = bcex.Message;
                }
                catch (NoMoneyException nmex)
                {
                    error = nmex.Message;
                }
                catch (NoTtlException ntex)
                {
                    error = ntex.Message;
                }
                catch (ObjectDoesNotExistException odnex)
                {
                    error = odnex.Message;
                }
                catch (FriendshipLeveTooLowException fltlex)
                {
                    error = fltlex.Message;
                }
            } while (isRunning);
        }

        protected void JumpTo()
        {
            Output.Clear();
            this.DisplayHeroStats();

            var locations = _locationBll.GetAllLocations();

            var menu = new Menu();
            menu.Add("Back to Hero menu", () => { }, ConsoleColor.DarkYellow);

            foreach (var loc in locations)
            {
                if (_hero.CurrentLocationId == loc.LocationId)
                {
                    menu.Add($"{loc.Name} (current)", () => { });
                }
                else
                {
                    menu.Add(loc.Name, () => { _heroBll.JumpTo(_hero, loc); });
                }
            }

            menu.Display();
        }

        protected void DoAction(ActionToDo action)
        {
            Output.WriteLine("Performing {0}", action.Name);
            _heroBll.ApplyAction(_hero, action);
            //Input.ReadKey();
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
                            menu.Add($"{improve.Name} using {improve.TtlToUse} hours", () =>
                            {
                                //Bll.BllFactory.Current.HeroBll.Improve(_hero, improve as ActionToImprove);
                                Output.WriteLine(ConsoleColor.Green, "You have improved {0}", improve);
                            });
                        }
                    }
                    else
                    {
                        Output.WriteLine(ConsoleColor.Red, "No improvments are available here");
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
            sb.AppendFormat("|{0,8}|{1,8}|{2,8}|{3,12}|{4,8}|{5,8}", "TTL", "IQ", "Charm", "Strength", "Money", "Day").AppendLine();
            sb.Append(" ".PadLeft(80, ' '));
            sb.AppendFormat("|{0,8}|{1,8}|{2,8}|{3,12}|{4,8}|{5,8}", _hero.Ttl, _hero.Iq, _hero.Charm, _hero.Strength, _hero.Money, String.Concat(_hero.Calendar.Day, ", ", _hero.Calendar.WeekDayShort));
            sb.AppendLine();
            sb.Append(" ".PadLeft(80, ' '));
            sb.Append("-".PadLeft(58, '-'));
            sb.AppendLine();

            string[] giftNames = _hero.Gifts?.Select(x => x.Name).Distinct().ToArray();
            for (int i = 0; i < giftNames.Length; i++)
            {
                sb.AppendFormat("| {0}: {1} ", giftNames[i], _hero.Gifts.Count(x => x.Name == giftNames[i]));
            }
            sb.AppendLine();

            Output.WriteLine(ConsoleColor.DarkGreen, sb.ToString());
        }
    }
}
