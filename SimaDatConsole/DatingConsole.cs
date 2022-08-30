using AvUtils;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Shared;
using SimaDatConsole.Models;
using System;

namespace SimaDatConsole
{
    internal class DatingConsole
    {
        private readonly IDatingBll _datingBll;

        public DatingConsole(IDatingBll datingBll)
        {
            _datingBll = datingBll ?? throw new ArgumentNullException(nameof(datingBll));
        }

        public void DoDating(DatingLocation datingLocation, Hero me)
        {
            bool isDating = true;
            DatingActionResult result = null;
            string errorMsg = "";
            var menu = new Menu();
            var actions = _datingBll.GetHeroActions(datingLocation);
            var girl = new Girl("Dating Girl", SimaDat.Models.Enums.FriendshipLevels.Friend);
            me.SpendMoney(-datingLocation.Price);
            _datingBll.JoinDating(me, girl, datingLocation);

            menu.Add("Quit dating", () => { isDating = false; }, ConsoleColor.DarkYellow);
            foreach (var a in actions)
            {
                errorMsg = "";
                result = null;
                menu.Add(a.Name, () => { result = DoDatingAction(datingLocation, a); });
            }

            do
            {
                Console.Clear();
                Output.WriteLine(ConsoleColor.DarkGray, "Probability to kiss is: {0:0.000#}", ProbabilityCalculator.ProbabilityToKiss(me.Charm, girl.FriendshipLevel));
                if (string.IsNullOrEmpty(errorMsg) == false)
                {
                    Output.WriteLine(ConsoleColor.Red, errorMsg);
                }
                else if (result != null)
                {
                    Output.WriteLine(result.Status ? ConsoleColor.Green : ConsoleColor.Red, result.Message);
                }
                Console.WriteLine("Kiss point {0}", datingLocation.KissPoints);
                menu.Display();
            } while (isDating);
        }

        private DatingActionResult DoDatingAction(DatingLocation datingLocation, ActionToDo a)
        {
            if (a is ActionToPresent actionToPresent)
            {
                return Present(datingLocation, actionToPresent);
            }
            else if (a is ActionToKiss)
            {
                return Kiss(datingLocation);
            }

            return new DatingActionResult
            {
                Status = false,
                Message = $"Do not know how to act `{a.Name}`"
            };
        }

        private DatingActionResult Kiss(DatingLocation datingLocation)
        {
            var result = new DatingActionResult();

            try
            {
                _datingBll.Kiss(datingLocation);
                result.Status = true;
                result.Message = "You've kissed her, she is your lover.";
            }
            catch (BadConditionException bcex)
            {
                result.Message = bcex.Message;
            }

            return result;
        }

        private DatingActionResult Present(DatingLocation datingLocation, ActionToPresent actionToPresent)
        {
            var result = new DatingActionResult();

            try
            {
                _datingBll.Present(datingLocation, actionToPresent.GiftType);
                result.Status = true;
                result.Message = "Presented...";
            }
            catch (ObjectDoesNotExistException obneex)
            {
                result.Message = obneex.Message;
            }

            return result;
        }
    }
}
