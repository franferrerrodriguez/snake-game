using System;
using System.Threading;

namespace Proyecto.classes
{
    public class Game
    {
        private static Board board;
        public enum GameMode { CLOSED, OPENED };
        private static GameMode game_mode;
        public enum Difficult { VERY_EASY, EASY, NORMAL, HARD, EXTREME, INSANE };
        private static Difficult difficult;

        public Game(Difficult difficult, GameMode gameMode)
        {
            SetDifficult(difficult);
            SetGameMode(gameMode);
        }

        public static GameMode GetGameMode()
        {
            return game_mode;
        }

        public static void SetGameMode(GameMode _game_mode)
        {
            game_mode = _game_mode;
        }

        public static Difficult GetDifficult()
        {
            return difficult;
        }

        public static void SetDifficult(Difficult _difficult)
        {
            difficult = _difficult;
        }

        public void Start()
        {
            board = new Board();

            while (true)
            {
                Thread.Sleep(board.GetSpeed());
                board.GetSnake().Move(board);
            }
        }

        public static void GameOver()
        {
            Console.WriteLine("FIN - Total movimientos: " + board.GetMovements() + " - Total puntos: " + board.GetScore());
            Environment.Exit(0);
            Console.ReadKey();
        }
    }
}