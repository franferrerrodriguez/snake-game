using Proyecto.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameProject
{
    class Program
    {
        public static void Main(string[] args)
        {
            // 40 - 90 (5 NIVELES)
            Game game = new Game(Game.Difficult.VERY_EASY, Game.GameMode.CLOSED);
            game.Start();
        }
    }
}
