using System;
using System.Threading;

namespace Proyecto
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // 30 - 80 (5 NIVELES)esto es nuevo
            Juego juego = new Juego(Juego.Difficult.VERY_EASY, Juego.GameMode.CLOSED);
            juego.Start();
        }

        class Juego
        {
            private static Board board;
            public enum GameMode { CLOSED, OPENED };
            private static GameMode gameMode;
            public enum Difficult { VERY_EASY, EASY, NORMAL, HARD, EXTREME, INSANE };
            private static Difficult difficult;

            public Juego(Difficult difficult, GameMode gameMode)
            {
                SetDifficult(difficult);
                SetGameMode(gameMode);
            }

            public static GameMode GetGameMode()
            {
                return gameMode;
            }

            public static void SetGameMode(GameMode _gameMode)
            {
                gameMode = _gameMode;
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
                    board.MoveSnake();
                }
            }

            public static void GameOver()
            {
                Console.WriteLine("FIN - Total movimientos: " + board.GetMovements() + " - Total puntos: " + board.GetScore());
                Environment.Exit(0);
                Console.ReadKey();
            }

        }

        class Board {
            public struct Snake
            {
                public Piece head;
                public Piece[] body;
            }
            private const int board_width = 40;
            private const int board_height = 20;
            private const int initial_snake_length = 3;
            private static Snake snake;
            public enum Direction { UP, RIGHT, DOWN, LEFT };
            private Direction keyboard_direction = Direction.RIGHT;
            private readonly Piece[,] board;
            private int speed;
            private int movements;
            private int score;
            private bool active_food;
            private bool active_special_food;
            private DateTime special_food_time;

            public Board()
            {
                Console.CursorVisible = false;
                board = new Piece[board_width, board_height];
                SetSpeed(Juego.GetDifficult());

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

                MakeSnake(initial_snake_length);
            }

            public void MakeSnake(int snake_length)
            {
                if(snake_length > 1)
                {
                    snake.body = new Piece[snake_length - 1];
                    for (int i = 0; i < snake_length - 1; i++)
                        snake.body[i] = GetBoardPiece(i, 0).SetType(Piece.Type.BODY);
                    snake.head = GetBoardPiece(snake_length - 1, 0).SetType(Piece.Type.HEAD);
                }
            }

            public void MoveSnake()
            {
                Direction direction = GetDirectionKeyboard();
                Direction direction_tmp;

                // Cogemos la pieza cabeza actual
                Piece actual_piece = GetBoardPiece(snake.head.GetX(), snake.head.GetY());

                // Cogemos la pieza cabeza siguiente como pieza actual
                actual_piece = NextPiece(direction, actual_piece);

                // Si la serpiente se encuentra con otra posición cuerpo, terminará el juego
                if (actual_piece.GetType() == Piece.Type.BODY)
                    Juego.GameOver();

                bool increase_snake = false;
                // Si el tipo de la pieza actual es FOOD significa que ha encontrado comida
                if (actual_piece.GetType() == Piece.Type.FOOD)
                {
                    // Si la serpiente encuentra comida, aumentamos el array snake.body una posición
                    Array.Resize(ref snake.body, snake.body.Length + 1);
                    // Seteamos increase_snake a true, informando que hay que incrementar el snake.body
                    increase_snake = true;
                    // Seteamos increase_snake a false, informando que hay que añadir más comida al tablero
                    active_food = false;
                    // Aumentamos puntuación del usuario
                    score ++;
                }

                // Comprobamos si existe o debemos asignar comida en el tablero
                UpdateFood();

                // Asignamos la pieza cabeza actual al array serpiente y pintamos
                snake.head = actual_piece.SetType(Piece.Type.HEAD).Draw();

                // Recorremos el cuerpo de la serpiente para actualizar estados y posiciones
                for (int i = increase_snake ? snake.body.Length - 2 : snake.body.Length - 1; i >= 0; i--)
                {
                    // Si la serpiente ha encontrado comida, la posición que asignará en el snake.body.i, será i + 1
                    int i_tmp = !increase_snake ? i : i + 1;

                    // Cogemos la pieza cuerpo actual
                    actual_piece = GetBoardPiece(snake.body[i].GetX(), snake.body[i].GetY());

                    // Última pieza cuerpo de la serpiente
                    if (i == 0)
                    {
                        // Si la serpiente no ha encontrado comida, la última posición será relleno y pintamos
                        actual_piece.SetType(Piece.Type.FILL).Draw();

                        // Si la serpiente ha encontrado comida, la posición en snake.body[0] (i - 1) será cuerpo y pintamos
                        if (increase_snake)
                            snake.body[i_tmp - 1] = actual_piece.SetType(Piece.Type.BODY).Draw();
                    }

                    // Guardamos dirección temporal de la pieza cuerpo actual para ser asinada 3 líneas más abajo
                    // De esta forma controlamos que cada pieza tenga una dirección independiente para que la serpiente tenga
                    // un recorrido correcto en el tablero siguiendo la dirección maestra (cabeza)
                    direction_tmp = actual_piece.GetDirection();

                    // Cogemos la pieza cuerpo siguiente como pieza actual
                    actual_piece = NextPiece(direction_tmp, actual_piece);

                    // Asignamos la pieza cuerpo actual al array serpiente y asignamos la dirección anterior y pintamos
                    snake.body[i_tmp] = actual_piece.SetType(Piece.Type.BODY).SetDirection(direction).Draw();

                    // Registramos la dirección de la pieza cuerpo anterior
                    direction = direction_tmp;
                }
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
                        if (keyboard_direction != Direction.UP)
                            keyboard_direction = Direction.DOWN;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (keyboard_direction != Direction.RIGHT)
                            keyboard_direction = Direction.LEFT;
                        break;
                    case ConsoleKey.RightArrow:
                        if (keyboard_direction != Direction.LEFT)
                            keyboard_direction = Direction.RIGHT;
                        break;
                    case ConsoleKey.UpArrow:
                        if (keyboard_direction != Direction.DOWN)
                            keyboard_direction = Direction.UP;
                        break;
                }

                movements++;

                return keyboard_direction;
            }

            public void UpdateFood()
            {
                if (!active_food)
                {
                    active_food = true;
                    Random rnd;
                    int x, y;

                    bool valid = false;
                    do
                    {
                        rnd = new Random();
                        x = rnd.Next(0, GetWidth());
                        y = rnd.Next(0, GetHeight());
                        if (GetBoardPiece(x, y).GetType() == Piece.Type.FILL)
                            valid = true;
                    } while (!valid);

                    board[x, y].SetType(Piece.Type.FOOD).Draw();
                }
            }

            public void UpdateSpecialFood()
            {
                if (!active_special_food)
                {
                    active_special_food = true;

                    /*
                    if (food_time == new DateTime())
                    {
                        food_time = DateTime.Now;
                        board[x, y].SetType(Piece.Type.FOOD).Draw();
                    }

                    if(food_time.AddSeconds(5) < DateTime.Now)
                        board[x, y].SetType(Piece.Type.FOOD).Draw();
                    */
                }
            }

            public void SetSpeed(Juego.Difficult difficult)
            {
                switch (difficult)
                {
                    case Juego.Difficult.VERY_EASY:
                        speed = 80;
                        break;
                    case Juego.Difficult.EASY:
                        speed = 70;
                        break;
                    case Juego.Difficult.NORMAL:
                        speed = 60;
                        break;
                    case Juego.Difficult.HARD:
                        speed = 50;
                        break;
                    case Juego.Difficult.EXTREME:
                        speed = 40;
                        break;
                    case Juego.Difficult.INSANE:
                        speed = 30;
                        break;
                    default:
                        speed = 30;
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

            public void SetScore(int score)
            {
                this.score = score;
            }

        }

        class Piece
        {
            private struct BoardCoords
            {
                public int x, y;

                public BoardCoords(int _x, int _y)
                {
                    x = _x;
                    y = _y;
                }
            }

            private struct CursorCoords
            {
                public int x0, y0, x1, y1;

                public CursorCoords(int _x0, int _y0, int _x1, int _y1)
                {
                    x0 = _x0;
                    y0 = _y0;
                    x1 = _x1;
                    y1 = _y1;
                }
            }

            public enum Type { HEAD, BODY, FILL, BORDER, FOOD, SPECIAL_FOOD };
            private BoardCoords boardCoords;
            private CursorCoords cursorCoords;
            private Board.Direction direction;
            private Type type;
            private ConsoleColor color;
            private static bool change_body_color;

            public Piece(int x, int y, int x0, int y0, int x1, int y1)
            {
                SetType(Type.FILL);
                SetDirection(Board.Direction.RIGHT);

                boardCoords = new BoardCoords(x, y);
                cursorCoords = new CursorCoords(x0, y0, x1, y1);
            }

            public Piece(Type type)
            {
                SetType(type);
            }

            public Piece SetDirection(Board.Direction direction)
            {
                this.direction = direction;
                return this;
            }

            public Board.Direction GetDirection()
            {
                return direction;
            }

            public Piece SetType(Type type)
            {
                this.type = type;
                switch (type)
                {
                    case Type.HEAD:
                        change_body_color = false;
                        color = ConsoleColor.DarkBlue;
                        break;
                    case Type.BODY:
                        color = change_body_color ? ConsoleColor.DarkGreen : ConsoleColor.DarkCyan;
                        change_body_color = !change_body_color;
                        break;
                    case Type.FILL:
                        color = ConsoleColor.Gray;
                        break;
                    case Type.BORDER:
                        color = ConsoleColor.DarkGray;
                        break;
                    case Type.FOOD:
                        color = ConsoleColor.DarkRed;
                        break;
                    case Type.SPECIAL_FOOD:
                        color = ConsoleColor.Yellow;
                        break;
                }
                return this;
            }

            public new Type GetType()
            {
                return type;
            }

            public int GetX()
            {
                return boardCoords.x;
            }

            public int GetY()
            {
                return boardCoords.y;
            }

            public Piece Draw()
            {
                Console.BackgroundColor = color;
                Console.SetCursorPosition(cursorCoords.x0, cursorCoords.y0);
                Console.Write(" ");
                Console.SetCursorPosition(cursorCoords.x1, cursorCoords.y1);
                Console.Write(" ");
                Console.SetCursorPosition(Board.GetWidth(), Board.GetHeight());
                return this;
            }

        }

    }
}





















