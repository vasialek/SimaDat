﻿using AvUtils;
using SimaDat.Core;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Items;

namespace SimaDatConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool isRunning = true;
            var menu = new Menu();
            var locationBll = BllFactory.Current.LocationBll;
            var heroBll = BllFactory.Current.HeroBll;
            var charactersBll = BllFactory.Current.CharactersBll;
            var datingBll = BllFactory.Current.DatingBll;

            try
            {
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
                locationBll.CreateDoorInLocation(home, square, Directions.North);
                // Square <-> Pub
                locationBll.CreateDoorInLocation(square, pub, Directions.North);
                // Square <-> Gym
                locationBll.CreateDoorInLocation(square, gym, Directions.East);
                // Square <-> School
                locationBll.CreateDoorInLocation(square, school, Directions.West);
                // Square <-> Girl house
                locationBll.CreateDoorInLocation(square, girlHouse, Directions.NorthEast);
                // Girls house <-> Lina room
                locationBll.CreateDoorInLocation(girlHouse, linaRoom, Directions.NorthWest);
                // Girls house <-> Anna room
                locationBll.CreateDoorInLocation(girlHouse, annaRoom, Directions.North);
                // Square <-> City center
                locationBll.CreateDoorInLocation(square, cityCenter, Directions.SouthEast);
                // School <-> Library
                locationBll.CreateDoorInLocation(school, library, Directions.North);
                // City center <-> Bazaar
                locationBll.CreateDoorInLocation(cityCenter, bazaar, Directions.SouthWest);
                // City center <-> Dock
                locationBll.CreateDoorInLocation(cityCenter, dock, Directions.South);
                // City center <-> Cafe
                locationBll.CreateDoorInLocation(cityCenter, cafe, Directions.SouthEast);

                // Girls
                Girl laura = new Girl
                {
                    Name = "Laura",
                    Appearance = new Appearance(165, 80, 60, 95) { Hair = Hairs.Black },
                    CurrentLocationId = pub.LocationId,
                };
                charactersBll.CreateGirl(laura);
                Girl lina = new Girl
                {
                    Name = "Lina",
                    Appearance = new Appearance(165, 97, 70, 95) { Hair = Hairs.Blond },
                };
                charactersBll.CreateGirl(lina);
                linaRoom.OwnerId = lina.CharacterId;
                Girl anna = new Girl
                {
                    Name = "Anna",
                    Appearance = new Appearance(175, 80, 60, 90) { Hair = Hairs.Red },
                    CurrentLocationId = pub.LocationId,
                };
                charactersBll.CreateGirl(anna);
                annaRoom.OwnerId = anna.CharacterId;

                var girlFamilar = new Girl("Familar girl", FriendshipLevels.Familar);
                girlFamilar.CurrentLocationId = home.LocationId;
                charactersBll.CreateGirl(girlFamilar);

                // Conditions
                school.SetEnterCondition("School is closed on weekend.", (Hero h) => { return h.Calendar.WeekDay < 6; });
                linaRoom.SetEnterCondition((Hero h) =>
                {
                    var owner = BllFactory.Current.LocationBll.GetOwnerOfLocation(linaRoom.LocationId);
                    return ((int)owner.FriendshipLevel >= (int)FriendshipLevels.Familar);
                });
                annaRoom.SetEnterCondition((Hero h) =>
                {
                    return (int)anna.FriendshipLevel >= (int)FriendshipLevels.Familar;
                });

                var me = new Hero();
                me.Name = "Lekha";
                me.CurrentLocationId = home.LocationId;
                me.ResetTtl();

                // Gifts
                for (int i = 0; i < 10; i++)
                {
                    me.Gifts.Add(new Gift { Name = "Test flower", GiftTypeId = GiftTypes.Flower, FirendshipPoints = 5 });
                }

                var console = new SdConsole(me, locationBll, charactersBll);
                var datingConsole = new DatingConsole(datingBll);
                var testDatingLocation = new DatingLocation("Test dating place", 100);

                menu.Add("Exit", () => { isRunning = false; }, ConsoleColor.Red);
                menu.Add("Hero menu", () => { console.HeroMenu(); }, ConsoleColor.Green);
                menu.Add("Hero", () => { console.DisplayHero(); }, ConsoleColor.Yellow);
                menu.Add("Move hero", () => { console.MoveHero(); });
                menu.Add("Improve hero", () => { console.ImproveHero(); });
                menu.Add("Display locations", () => { console.DisplayLocations(); });
                menu.Add("Display girls", () => { console.DisplayGirls(); });
                menu.Add($"Date with ", () => { datingConsole.DoDating(testDatingLocation, me); });

                do
                {
                    menu.Display();
                } while (isRunning);
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, ex.Message);
                Output.WriteLine(ex.ToString());
            }
        }
    }
}
