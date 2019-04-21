using AvUtils;
using SimaDat.Models.Actions;
using SimaDat.Models.Datings;
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

		public void DoDating(DatingLocation datingLocation)
		{
			bool isDating = true;
			var menu = new Menu();
			var actions = _datingBll.GetHeroActions(datingLocation);


			menu.Add("Quit dating", () => { isDating = false; }, ConsoleColor.DarkYellow);
			foreach (var a in actions)
			{
				menu.Add(a.Name, () => {
					if (a is ActionToPresent actionToPresent)
					{
						_datingBll.Present(datingLocation, actionToPresent.GiftType);
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
