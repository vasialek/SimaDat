﻿using AvUtils;
using SimaDat.Bll;
using SimaDat.Models;
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


            var console = new SdConsole(locationBll);


            menu.Add("Exit", () => { isRunning = false; }, ConsoleColor.Red);
            menu.Add("Display locations", () => { console.DisplayLocations(); });

            do
            {
                menu.Display();
            } while (isRunning);
        }
    }
}