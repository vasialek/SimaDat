using AvUtils;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Shared;
using System;

namespace SimaDatConsole
{
	internal class DatingConsole
	{
		private IDatingBll _datingBll = null;

		public DatingConsole(IDatingBll datingBll)
		{
			_datingBll = datingBll ?? throw new ArgumentNullException(nameof(datingBll));
		}

		public void DoDating(DatingLocation datingLocation, Hero me)
		{
			bool isDating = true;
			string errorMsg = "";
			var menu = new Menu();
			var actions = _datingBll.GetHeroActions(datingLocation);
			Girl g = new Girl("Dating Girl", SimaDat.Models.Enums.FriendshipLevels.Friend);
			me.SpendMoney(-datingLocation.Price);
			_datingBll.JoinDating(me, g, datingLocation);


			menu.Add("Quit dating", () => { isDating = false; }, ConsoleColor.DarkYellow);
			foreach (var a in actions)
			{
				errorMsg = "";
				menu.Add(a.Name, () => {
					if (a is ActionToPresent actionToPresent)
					{
						_datingBll.Present(datingLocation, actionToPresent.GiftType);
					}
					else if (a is ActionToKiss actionToKiss)
					{
						try
						{
							_datingBll.Kiss(datingLocation);
						}
						catch (BadConditionException bcex)
						{
							errorMsg = bcex.Message;
						}
					}
					else
					{
						errorMsg = $"Do not know how to act `{a.Name}`";
					}
				});
			}

			do
			{
				Console.Clear();
				Output.WriteLine(ConsoleColor.DarkGray, "Probability to kiss is: {0:0.000#}", ProbabilityCalculator.ProbabilityToKiss(me.Charm, g.FriendshipLevel));
				if (String.IsNullOrEmpty(errorMsg) == false)
				{
					Output.WriteLine(ConsoleColor.Red, errorMsg);
				}
				Console.WriteLine("Kiss point {0}", datingLocation.KissPoints);
				menu.Display();
			} while (isDating);
		}
	}
}
