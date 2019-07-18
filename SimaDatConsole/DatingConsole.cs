using AvUtils;
using SimaDat.Models.Actions;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
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
			var menu = new Menu();
			var actions = _datingBll.GetHeroActions(datingLocation);
			Girl g = new Girl("Dating Girl", SimaDat.Models.Enums.FriendshipLevels.Friend);
			me.SpendMoney(-datingLocation.Price);
			_datingBll.JoinDating(me, g, datingLocation);


			menu.Add("Quit dating", () => { isDating = false; }, ConsoleColor.DarkYellow);
			foreach (var a in actions)
			{
				menu.Add(a.Name, () => {
					if (a is ActionToPresent actionToPresent)
					{
						_datingBll.Present(datingLocation, actionToPresent.GiftType);
					}
					else if (a is ActionToKiss actionToKiss)
					{
						try
						{
							_datingBll.Kiss();
						}
						catch (BadConditionException bcex)
						{
							Output.WriteLine(ConsoleColor.Red, bcex.Message);
						}
					}
					else
					{
						Console.WriteLine("Do not know how to act `{0}`", a.Name);
					}
				});
			}

			do
			{
				System.Console.WriteLine("Kiss point {0}", datingLocation.KissPoints);
				menu.Display();
			} while (isDating);
		}
	}
}
