using Proyecto;
using Proyecto.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameProject.snake.exception
{
    class GameOverException : Exception
    {

        public GameOverException(Board board) : base("El juego ha finalizado.")
        {
            Game.SetGameOver(true);
            board.RefreshScore();
        }

        public GameOverException(Board board, string message) : base(String.Format("El juego ha finalizado por el siguiente motivo: {0}", message))
        {
            Game.SetGameOver(true);
            board.RefreshScore();
        }

        public GameOverException(string message) : base(String.Format("El juego ha finalizado por el siguiente motivo: {0}", message))
        {
            Game.SetGameOver(true);
        }

    }
}
