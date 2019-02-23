using System;
using Proyecto.classes;

namespace Proyecto
{
    public class Board
    {
        private const int board_width = 40;
        private const int board_height = 20;
        private Snake snake;
        public enum Direction { UP, RIGHT, DOWN, LEFT };
        private Direction keyboard_direction = Direction.RIGHT;
        private readonly Piece[,] board;
        private int speed;
        private int movements;
        private int score;
        private Food normal_food;
        private Food special_food;

        public Board()
        {
            Console.CursorVisible = false;
            board = new Piece[board_width, board_height];
            snake = new Snake();
            SetSpeed(Game.GetDifficult());

            normal_food = new NormalFood();
            special_food = new SpecialFood();

            int x0, x1, y0 = 0, y1 = 0;
            for (int y = 0; y < board_height; y++)
            {
                x0 = 0;
                x1 = 0;
                for (int x = 0; x < board_width; x++)
                {
                    SetBoardPiece(x, y, new Piece(x, y, x0++, y0, ++x1, y1).Draw());
                    x0++;
                    x1++;
                }
                y0++;
                y1++;
            }

            snake.Make(this);
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

            // Comprobamos si se produce una excepción por exceder límites del array
            try
            {
                piece = GetBoardPiece(x, y);
            }
            catch (IndexOutOfRangeException)
            {
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

            movements++;

            return keyboard_direction;
        }

        public Snake GetSnake()
        {
            return snake;
        }

        public void SetSpeed(Game.Difficult difficult)
        {
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
                    speed = 40;
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

        public int GetSpeed()
        {
            return speed;
        }

        public int GetMovements()
        {
            return movements;
        }

        public void SetMovements(int movements)
        {
            this.movements = movements;
        }

        public int GetScore()
        {
            return score;
        }

        public void SumScore(int sum_score)
        {
            score += sum_score;
        }

        public Food GetFood()
        {
            return normal_food;
        }

        public void SetFood(Food normal_food)
        {
            this.normal_food = normal_food;
        }

        public Food GetSpecialFood()
        {
            return special_food;
        }

        public void SetSpecialFood(Food special_food)
        {
            this.special_food = special_food;
        }
    }
}