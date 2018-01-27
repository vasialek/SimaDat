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
            var library = new Location("Library");
            locationBll.CreateLocation(library);
            // Ala barack :)
            var girlHouse = new Location("Girl house");
            locationBll.CreateLocation(girlHouse);
            // Girls in barack
            var linaRoom = new Location("Lina room");
            locationBll.CreateLocation(linaRoom);
            var annaRoom = new Location("Anna room");
            locationBll.CreateLocation(annaRoom);
            var cityCenter = new Location("City center");
            locationBll.CreateLocation(cityCenter);
            // Some job centers
            var bazaar = new Location("Bazaar");
            locationBll.CreateLocation(bazaar);
            var cafe = new Location("Cafe");
            locationBll.CreateLocation(cafe);
            var dock = new Location("Dock");
            locationBll.CreateLocation(dock);

            // Home <-> Square
            locationBll.CreateDoorInLocation(home, square, Models.Enums.Directions.North);
            // Square <-> Pub
            locationBll.CreateDoorInLocation(square, pub, Models.Enums.Directions.North);
            // Square <-> Gym
            locationBll.CreateDoorInLocation(square, gym, Models.Enums.Directions.East);
            // Square <-> School
            locationBll.CreateDoorInLocation(square, school, Models.Enums.Directions.West);
            // Square <-> Girl house
            locationBll.CreateDoorInLocation(square, girlHouse, Models.Enums.Directions.NorthEast);
            // Girls house <-> Lina room
            locationBll.CreateDoorInLocation(girlHouse, linaRoom, Models.Enums.Directions.NorthWest);
            // Girls house <-> Anna room
            locationBll.CreateDoorInLocation(girlHouse, annaRoom, Models.Enums.Directions.North);
            // Square <-> City center
            locationBll.CreateDoorInLocation(square, cityCenter, Models.Enums.Directions.SouthEast);
            // School <-> Library
            locationBll.CreateDoorInLocation(school, library, Models.Enums.Directions.North);
            // City center <-> Bazaar
            locationBll.CreateDoorInLocation(cityCenter, bazaar, Models.Enums.Directions.SouthWest);
            // City center <-> Dock
            locationBll.CreateDoorInLocation(cityCenter, dock, Models.Enums.Directions.South);
            // City center <-> Cafe
            locationBll.CreateDoorInLocation(cityCenter, cafe, Models.Enums.Directions.SouthEast);

            // Conditions
            school.SetEnterCondition("School is closed on weekend.", (Hero h) => { return h.Calendar.WeekDay < 6; });

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
