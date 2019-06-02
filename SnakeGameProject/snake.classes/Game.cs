using SnakeGameProject;
using SnakeGameProject.snake.classes;
using SnakeGameProject.snake.exception;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Proyecto.classes
{
    /*
     * Clase Game, encargada de todo lo relacionado con la partida en cada momento, dificultades, velocidad, tamaño, etc.
     */
    public class Game
    {
        private static Board board;
        public enum GameMode { CLOSED, OPENED };
        private static GameMode game_mode;
        public enum Difficult { VERY_EASY, EASY, NORMAL, HARD, EXTREME, INSANE };
        private static Difficult difficult;
        public enum SizeWindow { BIG, MEDIUM, SMALL };
        private static SizeWindow size_window;
        private static bool game_over;
        private static string username;
        private static int speed;
        private static int movements;
        private static int score;
        private static List<Punctuation> punctuations;

        public Game(string _username, Difficult difficult, GameMode _game_mode, SizeWindow _size_window)
        {
            Console.Clear();
            Console.CursorVisible = false;
            SetDifficult(difficult);
            movements = 0;
            score = 0;
            game_over = false;
            username = _username.ToUpper();
            game_mode = _game_mode;
            size_window = _size_window;
            board = new Board();
            board.RefreshScore(0);
            try
            {
                while (true)
                {
                    Thread.Sleep(speed);
                    board.GetSnake().Refresh();
                }
            } catch (GameOverException)
            {
                Console.ReadKey();
                punctuations.Add(new Punctuation(DateTime.Now, username, score, movements));
                Util.Save(punctuations);
            }

        }

        public static string GetUsername()
        {
            return username;
        }

        public static void SetUsername(string _username)
        {
            username = _username;
        }

        public static GameMode GetGameMode()
        {
            return game_mode;
        }

        public static SizeWindow GetSizeWindow()
        {
            return size_window;
        }

        public static bool GetGameOver()
        {
            return game_over;
        }

        public static void SetGameOver(bool _game_over)
        {
            game_over = _game_over;
        }

        public static int GetScore()
        {
            return score;
        }

        public static void SumScore(int _score)
        {
            score += _score;
        }

        public static int GetMovements()
        {
            return movements;
        }

        public static void SumMovement()
        {
            movements += 1;
        }

        public static List<Punctuation> GetPunctuations()
        {
            return punctuations;
        }

        public static void SetPunctuations(List<Punctuation> _punctuations)
        {
            punctuations = _punctuations;
        }

        // Establece la velocidad del juego a partir del enum Difficult
        public static void SetDifficult(Difficult _difficult)
        {
            difficult = _difficult;
            switch (difficult)
            {
                case Game.Difficult.VERY_EASY:
                    speed = 90;
                    break;
                case Game.Difficult.EASY:
                    speed = 80;
                    break;
                case Game.Difficult.NORMAL:
                    speed = 70;
                    break;
                case Game.Difficult.HARD:
                    speed = 60;
                    break;
                case Game.Difficult.EXTREME:
                    speed = 50;
                    break;
                case Game.Difficult.INSANE:
                    speed = 40;
                    break;
                default:
                    speed = 90;
                    break;
            }
        }

    }
}