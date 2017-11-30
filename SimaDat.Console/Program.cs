using AvUtils;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;
            var menu = new Menu();
            var locationBll = BllFactory.Current.LocationBll;
            var heroBll = BllFactory.Current.HeroBll;
            var charactersBll = BllFactory.Current.CharactersBll;


            var home = new Location(100, "Home");
            locationBll.CreateLocation(home);
            var square = new Location("Square");
            locationBll.CreateLocation(square);
            var pub = new Location("Pub");
            locationBll.CreateLocation(pub);
            var gym = new Location("Gym");
            locationBll.CreateLocation(gym);
            var school = new Location("School");
            locationBll.CreateLocation(school);

            // Home <-> Square
            locationBll.CreateDoorInLocation(home, square, Models.Enums.Directions.North);
            // Square <-> Pub
            locationBll.CreateDoorInLocation(square, pub, Models.Enums.Directions.North);
            // Square <-> Gym
            locationBll.CreateDoorInLocation(square, gym, Models.Enums.Directions.East);
            // Square <-> School
            locationBll.CreateDoorInLocation(square, school, Models.Enums.Directions.West);

            // Girls
            Girl laura = new Girl
            {
                Name = "Laura",
                Appearance = new Appearance(165, 80, 60, 95) { Hair = Models.Enums.Hairs.Black },
                CurrentLocationId = pub.LocationId,
            };
            charactersBll.CreateGirl(laura);

            var me = new Hero();
            me.Name = "Lekha";
            me.CurrentLocationId = home.LocationId;
            me.ResetTtl();
            
            var console = new SdConsole(me, locationBll, charactersBll);


            menu.Add("Exit", () => { isRunning = false; }, ConsoleColor.Red);
            menu.Add("Hero menu", () => { console.HeroMenu(); }, ConsoleColor.Green);
            menu.Add("Hero", () => { console.DisplayHero(); }, ConsoleColor.Yellow);
            menu.Add("Move hero", () => { console.MoveHero(); });
            menu.Add("Improve hero", () => { console.ImproveHero(); });
            menu.Add("Display locations", () => { console.DisplayLocations(); });
            menu.Add("Display girls", () => { console.DisplayGirls(); });

            do
            {
                menu.Display();
            } while (isRunning);
        }
    }
}
