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
            Juego juego = new Juego(Juego.Difficult.VERY_EASY, Juego.GameMode.CLOSED);
            juego.Start();
        }
    }
}





















