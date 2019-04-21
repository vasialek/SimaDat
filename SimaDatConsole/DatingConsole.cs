using AvUtils;
using SimaDat.Models.Datings;
using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Console
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
					System.Console.WriteLine(a.Name);
				});
			}

			do
			{
				menu.Display();
			} while (isDating);
		}
	}
}
