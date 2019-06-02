using Proyecto.classes;
using SnakeGameProject.snake.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGameProject
{
    /*
     * Clase Program, clase principal donde se incluye el Main, encargada de gestionar el menú principal.
     */
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Snake Game - By Francisco José Ferrer Rodríguez";

            Game.SetPunctuations(Util.Load());
            Game.Difficult difficult = Game.Difficult.VERY_EASY;
            Game.GameMode game_mode = Game.GameMode.OPENED;
            Game.SizeWindow size_window = Game.SizeWindow.BIG;

            string[] menu_options = { " 1. PLAY ", " 2. DIFFICULT ", " 3. GAME MODE ", " 4. SIZE WINDOW ", " 5. SHOW PUNCTUATIONS ", " 6. EXIT " };
            string[] menu_option_1 = { " 1. VERY EASY ", " 2. EASY ", " 3. NORMAL ", " 4. HARD ", " 5. EXTREME ", " 6. INSANE ", " 7. BACK " };
            string[] menu_option_2 = { " 1. OPENED ", " 2. CLOSED ", " 3. BACK " };
            string[] menu_option_3 = { " 1. BIG ", " 2. MEDIUM ", " 3. SMALL ", " 4. BACK " };
            string[] menu_option_username = { " 1. BACK " };

            string option;
            while(true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Util.DrawSnakeLogo();
                Util.DrawAuthorLogo();
                Util.ShowMenu(menu_options);
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        do
                        {
                            Util.ShowMenu(menu_option_username, "Set username to play: ");
                            option = Console.ReadLine();
                            if(!option.Equals("") && !option.Equals("1"))
                            {
                                new Game(option, difficult, game_mode, size_window);
                                option = "1";
                            }
                        } while (!option.Equals("1"));
                        break;
                    case "2":
                        do
                        {
                            Util.ShowMenu(menu_option_1);
                            option = Console.ReadLine();
                            switch (option)
                            {
                                case "1":
                                    difficult = Game.Difficult.VERY_EASY;
                                    option = "7";
                                    break;
                                case "2":
                                    difficult = Game.Difficult.EASY;
                                    option = "7";
                                    break;
                                case "3":
                                    difficult = Game.Difficult.NORMAL;
                                    option = "7";
                                    break;
                                case "4":
                                    difficult = Game.Difficult.HARD;
                                    option = "7";
                                    break;
                                case "5":
                                    difficult = Game.Difficult.EXTREME;
                                    option = "7";
                                    break;
                                case "6":
                                    difficult = Game.Difficult.INSANE;
                                    option = "7";
                                    break;
                            }
                        } while (!option.Equals("7"));
                        break;
                    case "3":
                        do
                        {
                            Util.ShowMenu(menu_option_2);
                            option = Console.ReadLine();
                            switch (option)
                            {
                                case "1":
                                    game_mode = Game.GameMode.OPENED;
                                    option = "3";
                                    break;
                                case "2":
                                    game_mode = Game.GameMode.CLOSED;
                                    option = "3";
                                    break;
                            }
                        } while (!option.Equals("3"));
                        break;
                    case "4":
                        do
                        {
                            Util.ShowMenu(menu_option_3);
                            option = Console.ReadLine();
                            switch (option)
                            {
                                case "1":
                                    size_window = Game.SizeWindow.BIG;
                                    option = "4";
                                    break;
                                case "2":
                                    size_window = Game.SizeWindow.MEDIUM;
                                    option = "4";
                                    break;
                                case "3":
                                    size_window = Game.SizeWindow.SMALL;
                                    option = "4";
                                    break;
                            }
                        } while (!option.Equals("4"));
                        break;
                    case "5":
                        Util.ShowPunctuations();
                        break;
                    case "6":
                        Environment.Exit(1);
                        break;
                }
            }
            
        }
    }
}
