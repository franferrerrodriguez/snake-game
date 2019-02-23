using System;
using System.Threading;
using Proyecto.classes;

namespace Proyecto
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // 40 - 90 (5 NIVELES)
            Game game = new Game(Game.Difficult.VERY_EASY, Game.GameMode.CLOSED);
            game.Start();
        }
    }
}





















