using System;
using Proyecto.classes;

namespace Proyecto
{
    public class Snake
    {
        public Snake()
        {
        }

        private Piece head;
        private Piece[] body;
        private const int initial_snake_length = 3;

        public Piece GetHead()
        {
            return head;
        }

        public void SetHead(Piece head)
        {
            this.head = head;
        }

        public Piece[] GetBodyArray()
        {
            return body;
        }

        public void SetBodyArray(Piece[] body)
        {
            this.body = body;
        }

        public Piece GetBodyPos(int pos)
        {
            return body[pos];
        }

        public void SetBodyPos(int pos, Piece piece)
        {
            body[pos] = piece;
        }

        public void Resize(int n)
        {
            Array.Resize(ref body, GetBodyArray().Length + n);
        }

        public void Make(Board board)
        {
            SetBodyArray(new Piece[initial_snake_length - 1]);
            for (int i = 0; i < initial_snake_length - 1; i++)
                SetBodyPos(i, board.GetBoardPiece(i, 0).SetType(Piece.Type.BODY));

            SetHead(board.GetBoardPiece(initial_snake_length - 1, 0).SetType(Piece.Type.HEAD));
        }

        public void Move(Board board)
        {
            Board.Direction direction = board.GetDirectionKeyboard();
            Board.Direction direction_tmp;

            // Cogemos la pieza cabeza actual
            Piece actual_piece = board.GetBoardPiece(GetHead().GetX(), GetHead().GetY());

            // Cogemos la pieza cabeza siguiente como pieza actual
            actual_piece = board.NextPiece(direction, actual_piece);

            // Si la serpiente se encuentra con otra posición cuerpo, terminará el juego
            if (actual_piece.GetType().Equals(Piece.Type.BODY))
                Game.GameOver();

            bool increase_snake = false;
            // Si el tipo de la pieza actual es FOOD significa que ha encontrado comida
            if (actual_piece.GetType().Equals(Piece.Type.NORMAL_FOOD))
            {
                // Si la serpiente encuentra comida, aumentamos el array snake.body una posición
                Resize(1);

                // Seteamos increase_snake a true, informando que hay que incrementar el snake.body
                increase_snake = true;

                // Seteamos increase_snake a false, informando que ya no existe comida en el tablero
                board.GetFood().SetActiveFood(false);

                // Aumentamos puntuación del usuario en 1
                board.SumScore(1);
            }

            // Si el tipo de la pieza actual es FOOD significa que ha encontrado comida especial
            if (actual_piece.GetType().Equals(Piece.Type.SPECIAL_FOOD))
            {
                // Si la serpiente encuentra comida especial, aumentamos el array snake.body una posición
                Resize(1);

                // Seteamos increase_snake a true, informando que hay que incrementar el snake.body
                increase_snake = true;

                // Seteamos increase_snake a false, informando que ya no existe comida especial en el tablero
                board.GetSpecialFood().SetActiveFood(false).SetTime(DateTime.Now);

                // Seteamos de nuevo la fecha actual para que empiece a contar el tiempo para la salida de la nueva comida especial
                board.GetSpecialFood();

                // Aumentamos puntuación del usuario en 5
                board.SumScore(5);
            }

            // Comprobamos si existe o debemos asignar comida en el tablero
            board.GetFood().UpdateFood(board);

            // Comida especial
            board.GetSpecialFood().UpdateFood(board);

            // Asignamos la pieza cabeza actual al array serpiente y pintamos
            SetHead(actual_piece.SetType(Piece.Type.HEAD).Draw());

            // Recorremos el cuerpo de la serpiente para actualizar estados y posiciones
            for (int i = increase_snake ? GetBodyArray().Length - 2 : GetBodyArray().Length - 1; i >= 0; i--)
            {
                // Si la serpiente ha encontrado comida, la posición que asignará en el snake.body.i, será i + 1
                int i_tmp = !increase_snake ? i : i + 1;

                // Cogemos la pieza cuerpo actual
                actual_piece = board.GetBoardPiece(GetBodyPos(i).GetX(), GetBodyPos(i).GetY());

                // Última pieza cuerpo de la serpiente
                if (i.Equals(0))
                {
                    // Si la serpiente no ha encontrado comida, la última posición será relleno y pintamos
                    actual_piece.SetType(Piece.Type.FILL).Draw();

                    // Si la serpiente ha encontrado comida, la posición en snake.body[0] (i - 1) será cuerpo y pintamos
                    if (increase_snake)
                        SetBodyPos(i_tmp - 1, actual_piece.SetType(Piece.Type.BODY).Draw());
                }

                // Guardamos dirección temporal de la pieza cuerpo actual para ser asinada 3 líneas más abajo
                // De esta forma controlamos que cada pieza tenga una dirección independiente para que la serpiente tenga
                // un recorrido correcto en el tablero siguiendo la dirección maestra (cabeza)
                direction_tmp = actual_piece.GetDirection();

                // Cogemos la pieza cuerpo siguiente como pieza actual
                actual_piece = board.NextPiece(direction_tmp, actual_piece);

                // Asignamos la pieza cuerpo actual al array serpiente y asignamos la dirección anterior y pintamos
                SetBodyPos(i_tmp, actual_piece.SetType(Piece.Type.BODY).SetDirection(direction).Draw());

                // Registramos la dirección de la pieza cuerpo anterior
                direction = direction_tmp;
            }
        }
    }
}