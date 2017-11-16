using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvUtils
{

    /// <summary>
    /// Class to display text-based menu in console application.
    /// <code>new Menu().Add("Print", () => { Console.WriteLine("Hello, menu"); }).Display();</code>
    /// </summary>
    public class Menu
    {
        private IList<Option> Options { get; set; }

        public Menu()
        {
            Options = new List<Option>();
        }

        public void Display()
        {
            Console.WriteLine();
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i].Color != ConsoleColor.White)
                {
                    Console.ForegroundColor = Options[i].Color;
                    Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("{0}. {1}", i + 1, Options[i].Name); 
                }
            }
            Console.WriteLine();
            int choice = Input.ReadInt("Choose an option:", min: 1, max: Options.Count);

            Options[choice - 1].Callback();
        }

        public Menu Add(string option, Action callback)
        {
            return Add(new Option(option, callback, ConsoleColor.White));
        }

        public Menu Add(string option, Action callback, ConsoleColor color)
        {
            return Add(new Option(option, callback, color));
        }

        public Menu Add(Option option)
        {
            Options.Add(option);
            return this;
        }

        public bool Contains(string option)
        {
            return Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
        }

        public void UpdateName(int pos, string optionName)
        {
            if (pos >= 0 && pos < Options.Count)
            {
                Options[pos].SetName(optionName);
            }
        }
    }

    public static class Input
    {

        public static ConsoleKeyInfo ReadKey(string prompt = "Press any key to continue . . .")
        {
            Output.DisplayPrompt(prompt);
            return Console.ReadKey(false);
        }

        public static int ReadInt(string prompt, int min, int max, int? defaultValue = null)
        {
            Output.DisplayPrompt(prompt);
            return ReadInt(min, max, defaultValue);
        }

        public static int ReadInt(int min, int max, int? defaultValue = null)
        {
            int value = ReadInt(defaultValue);

            while (value < min || value > max)
            {
                Output.DisplayPrompt("Please enter an integer between {0} and {1} (inclusive)", min, max);
                value = ReadInt(defaultValue);
            }

            return value;
        }

        public static int ReadInt(int? defaultValue = null)
        {
            int value;
            bool wasError = false;
            string input = null;

            do
            {
                if (wasError)
                {
                    Output.DisplayPrompt("Please enter an integer");
                }

                input = Console.ReadLine();
                if (String.IsNullOrEmpty(input) && defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                wasError = true;
            } while (int.TryParse(input, out value) == false);

            //while (!int.TryParse(input, out value))
            //{
            //    Output.DisplayPrompt("Please enter an integer");
            //    input = Console.ReadLine();
            //}

            return value;
        }

        public static string ReadString(string prompt)
        {
            Output.DisplayPrompt(prompt);
            return Console.ReadLine();
        }

        public static DateTime ReadDate(string prompt, DateTime? defaultDate = null)
        {
            DateTime dt;
            string s = null;
            do
            {
                s = Input.ReadString(prompt);
                if (String.IsNullOrEmpty(s) && defaultDate.HasValue)
                {
                    return defaultDate.Value;
                }
            } while (DateTime.TryParse(s, out dt) == false);
            return dt;
        }

        public static bool ReadBool(string prompt)
        {
            bool? b = null; ;
            string s;

            do
            {
                s = Input.ReadString(prompt);
                switch (s.ToLower())
                {
                    case "y":
                    case "yes":
                    case "t":
                        b = true;
                        break;
                    case "n":
                    case "no":
                    case "f":
                        b = false;
                        break;
                }
            } while (b == null);

            return b.Value;
        }
    }


    public static class Output
    {
        public static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void DisplayPrompt(string format, params object[] args)
        {
            format = String.Concat(format.Trim(), " ");
            Console.Write(format, args);
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }

    public class Option
    {
        public string Name { get; private set; }
        public Action Callback { get; private set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.White;

        public Option(string name, Action callback)
            : this(name, callback, ConsoleColor.White)
        {
        }

        public Option(string name, Action callback, ConsoleColor color)
        {
            Name = name;
            Callback = callback;
            Color = color;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
