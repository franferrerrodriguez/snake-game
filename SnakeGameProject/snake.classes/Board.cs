using System;
using System.Collections.Generic;
using Proyecto.classes;
using SnakeGameProject.snake.exception;

namespace Proyecto
{
    /*
     * Clase Board, encargada del manejo del tablero del juego
     */
    public class Board
    {
        private Snake snake;
        private static int board_width = 50;
        private static int board_height = 25;
        private static int board_margin_top = 2;
        public enum Direction { UP, RIGHT, DOWN, LEFT };
        private Direction keyboard_direction = Direction.RIGHT;
        private readonly Piece[,] board;
        private static Food normal_food;
        private static Food special_food;
        private const int initial_snake_length = 3;

        public Board()
        {
            Console.CursorVisible = false;
            SetSizeWindow(Game.GetSizeWindow());
            board = new Piece[board_width, board_height];
            normal_food = new NormalFood();
            special_food = new SpecialFood();
            MakeBoardPiece();
        }
        public void MakeBoardPiece()
        {
            SetBorder();
            int x0, x1, y0 = board_margin_top + 1, y1 = board_margin_top + 1;
            for (int y = 0; y < board_height; y++)
            {
                x0 = 2;
                x1 = 2;
                for (int x = 0; x < board_width; x++)
                {
                    SetBoardPiece(x, y, new Piece(x, y, x0++, y0, ++x1, y1).Draw());
                    x0++;
                    x1++;
                }
                y0++;
                y1++;
            }
            MakeSnakeBoard();
        }

        public void SetBorder()
        {
            if (Game.GetGameMode().Equals(Game.GameMode.CLOSED))
                Console.BackgroundColor = ConsoleColor.DarkGray;
            else
                Console.BackgroundColor = ConsoleColor.White;
            List<int> lx = new List<int>() { 0, 1, board_width * 2 + 3, board_width * 2 + 2 };
            List<int> ly = new List<int>() { 0 + board_margin_top, board_height + board_margin_top + 1 };
            for (int y = 0 + board_margin_top; y < board_height + board_margin_top + 2; y++)
                for (int x = 0; x < board_width * 2 + 4; x++)
                    if (lx.Contains(x) || ly.Contains(y))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(" ");
                    }
        }

        public void MakeSnakeBoard()
        {
            snake = new Snake(this);
            for (int i = initial_snake_length - 1; i >= 0; i--)
                snake.SetSnakePiece(GetBoardPiece(i, 0));
        }

        public Piece NextPiece(Direction direction, Piece piece)
        {
            int x = 0;
            int y = 0;
            int x_limit = 0;
            int y_limit = 0;
            switch (direction)
            {
                case Direction.UP:
                    x = piece.GetX();
                    y = piece.GetY() - 1;
                    x_limit = x;
                    y_limit = GetHeight() - 1;
                    break;
                case Direction.RIGHT:
                    x = piece.GetX() + 1;
                    y = piece.GetY();
                    x_limit = 0;
                    y_limit = y;
                    break;
                case Direction.DOWN:
                    x = piece.GetX();
                    y = piece.GetY() + 1;
                    x_limit = x;
                    y_limit = 0;
                    break;
                case Direction.LEFT:
                    x = piece.GetX() - 1;
                    y = piece.GetY();
                    x_limit = GetWidth() - 1;
                    y_limit = y;
                    break;
            }

            try
            {
                piece = GetBoardPiece(x, y);
            }
            catch (IndexOutOfRangeException)
            {
                if (Game.GetGameMode().Equals(Game.GameMode.CLOSED))
                    throw new GameOverException(this);
                else
                    piece = GetBoardPiece(x_limit, y_limit);
            }

            return piece;
        }

        public Direction GetDirectionKeyboard()
        {
            if (!Console.KeyAvailable)
                return keyboard_direction;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (keyboard_direction != Direction.UP)
                        keyboard_direction = Direction.DOWN;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (keyboard_direction != Direction.RIGHT)
                        keyboard_direction = Direction.LEFT;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (keyboard_direction != Direction.LEFT)
                        keyboard_direction = Direction.RIGHT;
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    if (keyboard_direction != Direction.DOWN)
                        keyboard_direction = Direction.UP;
                    break;
            }

            Game.SumMovement();

            return keyboard_direction;
        }

        public Snake GetSnake()
        {
            return snake;
        }

        public void SetSizeWindow(Game.SizeWindow size_window)
        {
            switch (size_window)
            {
                case Game.SizeWindow.BIG:
                    board_width = 50;
                    board_height = 25;
                    break;
                case Game.SizeWindow.MEDIUM:
                    board_width = 40;
                    board_height = 20;
                    break;
                case Game.SizeWindow.SMALL:
                    board_width = 30;
                    board_height = 15;
                    break;
                default:
                    board_width = 50;
                    board_height = 25;
                    break;
            }
        }

        public void SetBoardPiece(int x, int y, Piece piece)
        {
            board[x, y] = piece;
        }

        public Piece GetBoardPiece(int x, int y)
        {
            return board[x, y];
        }

        public static int GetWidth()
        {
            return board_width;
        }

        public static int GetHeight()
        {
            return board_height;
        }

        public void RefreshScore(int sum_score = 0)
        {
            Game.SumScore(sum_score);
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int y = 0; y < board_margin_top; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < GetWidth() * 2 + 4; x++)
                    Console.Write(" ");
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(String.Format(" {0} - TOTAL POINTS: {1} ", Game.GetUsername(), Game.GetScore()));
            if (Game.GetGameOver())
            {
                Console.WriteLine(" TOTAL MOVEMENTS: " + Game.GetMovements() + " ");

                string text1 = "GAME OVER";
                Console.SetCursorPosition(GetWidth() - text1.Length / 2, 0);
                Console.Write(text1);

                string text2 = "Press any key to return to the main menu...";
                Console.SetCursorPosition(GetWidth() - text2.Length / 2, 1);
                Console.Write(text2);
            }
            
        }

        public Food GetFood()
        {
            return normal_food;
        }

        public Food GetSpecialFood()
        {
            return special_food;
        }

    }
}