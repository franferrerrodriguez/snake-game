using Proyecto.classes;
using SnakeGameProject.snake.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameProject
{    /*
     * Clase Util encargada de generar utilidades adicionales al juego
     */
    static class Util
    {
        private const string ruta_fichero = "punctuations.txt";
        private const char delimitador = ';';

        public static void DrawSnakeLogo()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
                  ██████  ███▄    █  ▄▄▄       ██ ▄█▀▓█████
                ▒██    ▒  ██ ▀█   █ ▒████▄     ██▄█▒ ▓█   ▀                  ░░▒▒░░▒▒░░▓▓
                ░ ▓██▄   ▓██  ▀█ ██▒▒██  ▀█▄  ▓███▄░ ▒███                    ▒▒
                  ▒   ██▒▓██▒  ▐▌██▒░██▄▄▄▄██ ▓██ █▄ ▒▓█  ▄                  ░░
                ▒██████▒▒▒██░   ▓██░ ▓█   ▓██▒▒██▒ █▄░▒████░░▒▒░░▒▒          ▒▒
                ▒ ▒▓▒ ▒ ░░ ▒░   ▒ ▒  ▒▒   ▓▒█░▒ ▒▒ ▓▒░░ ▒░ ░     ░░          ░░
                ░ ░▒  ░ ░░ ░░   ░ ▒░  ▒   ▒▒ ░░ ░▒ ▒░ ░ ░  ░     ▒▒░░▒▒░░▒▒░░▒▒
                ░  ░  ░     ░   ░ ░   ░   ▒   ░ ░░ ░    ░  ░
                      ░           ░       ░  ░░  ░      ░
            ");
        }

        public static void DrawAuthorLogo()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"


                ______        ______                    _                   ___              _     
                | ___ \       |  ___|                  (_)                 |_  |            //   
                | |_/ /_   _  | |_ _ __ __ _ _ __   ___ _ ___  ___ ___       | | ___  ___  ___ 
                | ___ \ | | | |  _| '__/ _` | '_ \ / __| / __|/ __/ _ \      | |/ _ \/ __|/ _ \ 
                | |_/ / |_| | | | | | | (_| | | | | (__| \__ \ (_| (_) | /\__/ / (_) \__ \  __/ 
                \____/ \__, | \_| |_|  \__,_|_| |_|\___|_|___/\___\___/  \____/ \___/|___/\___| 
                ______  __/ |                ______          _       _                           
                |  ___||___/                 | ___ \        | |     //                          
                | |_ ___ _ __ _ __ ___ _ __  | |_/ /___   __| |_ __ _  __ _ _   _  ___ ____     
                |  _/ _ \ '__| '__/ _ \ '__| |    // _ \ / _` | '__| |/ _` | | | |/ _ \_  /     
                | ||  __/ |  | | |  __/ |    | |\ \ (_) | (_| | |  | | (_| | |_| |  __// /      
                \_| \___|_|  |_|  \___|_|    \_| \_\___/ \__,_|_|  |_|\__, |\__,_|\___/___|     
                                                                       __/ |
                                                                       |___/
            ");
        }

        public static void ClearLineMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 11);
            for (int i = 0; i < 115; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, 13);
            for (int i = 0; i < 115; i++)
                Console.Write(" ");
        }

        public static void ShowMenu(string[] options, string message = "Choose an option: ")
        {
            ClearLineMenu();

            Console.SetCursorPosition(16, 11);

            if(options != null)
            {
                foreach (string option in options)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(option);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(16, 13);
            Console.CursorVisible = true;
            Console.Write(message);
        }

        public static void ShowPunctuations()
        {
            Console.Clear();

            Console.WriteLine(String.Format("{0,20}{1,30}{2,20}{3,20}", "DATE", "USERNAME", "PUNCTUATION", "MOVEMENTS"));

            Game.GetPunctuations().Sort((a, b) => b.CompareTo(a));
            foreach (Punctuation p in Game.GetPunctuations())
                Console.WriteLine(p.ToString());

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        public static List<Punctuation> Load()
        {
            List<Punctuation> punctuations = new List<Punctuation>();

            // Comprobamos si el fichero existe en la ruta indicada
            if (File.Exists(ruta_fichero))
            {
                StreamReader fichero = File.OpenText(ruta_fichero);

                using (StreamReader rd = new StreamReader(ruta_fichero, Encoding.Default))
                {
                    // Recorremos las líneas del fichero
                    string linea;
                    int num_linea = 1;
                    do
                    {
                        linea = rd.ReadLine();
                        if (linea != null)
                        {
                            string[] split = linea.Split(delimitador);
                            bool b1 = DateTime.TryParse(split[0], out DateTime date);
                            string username = split[1];
                            bool b2 = int.TryParse(split[2], out int punctuation);
                            bool b3 = int.TryParse(split[3], out int movements);
                            if(b1 && b2 && b3)
                                punctuations.Add(new Punctuation(date, username, punctuation, movements));
                        }
                        num_linea++;
                    } while (linea != null);
                }

                fichero.Close();
            }

            return punctuations;
        }

        public static void Save(List<Punctuation> punctuations)
        {
            StreamWriter fichero = new StreamWriter(new FileStream(ruta_fichero, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8);

            foreach (Punctuation p in punctuations)
                fichero.WriteLine(String.Format("{0};{1};{2};{3}", p.GetDate(), p.GetUsername(), p.GetPunctuation(), p.GetMovements()));

            fichero.Close();
        }

    }

}