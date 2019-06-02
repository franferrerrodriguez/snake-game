using System;
using System.Collections.Generic;
using Proyecto.classes;
using SnakeGameProject.snake.exception;

namespace Proyecto
{
    /*
     * Clase Snake formada por objetos de tipo Piece, encargada de gestionar el tamaño, posiciones de la serpiente, dirección, etc.
     */
    public class Snake
    {
        private List<Piece> snake;
        private Board board;
        private Board.Direction direction;

        public Snake(Board board)
        {
            this.board = board;
            snake = new List<Piece>();
        }

        public void SetSnakePiece(Piece snake)
        {
            if (this.snake.Count.Equals(0))
                this.snake.Add(snake.SetType(Piece.Type.HEAD));
            else
                this.snake.Add(snake.SetType(Piece.Type.BODY));
        }

        public Piece GetHead()
        {
            return snake[0];
        }

        public Piece SetHead(Piece head)
        {
            return snake[0] = head.SetDirection(direction).SetType(Piece.Type.HEAD);
        }

        public List<Piece> GetBody()
        {
            return snake;
        }

        public Piece SetBody(int pos, Piece piece)
        {
            piece.SetType(Piece.Type.BODY);
            if (pos.Equals(1))
                piece.SetDirection(GetHead().GetDirection());
            return snake[pos] = piece;
        }

        public void MoveSnake(bool increase_snake = false)
        {
            // Última pieza de la serpiente
            Piece snake_tail = snake[snake.Count - 1];

            // Asignamos la siguiente posición a la cabeza actual para mover serpiente
            SetHead(board.NextPiece(direction, GetHead())).Draw();

            // Movemos el resto del cuerpo de la serpiente
            for(int i = 1; i < snake.Count; i++)
                SetBody(i, board.NextPiece(snake[i].GetDirection(), snake[i])).Draw();
            
            if (increase_snake) // Si increase_snake = true, añadimos un elemento a la lista snake
                snake.Add(snake_tail.SetType(Piece.Type.BODY));
            else // Si increase_snake = false, ponemos la cola a tipo FILL (Relleno)
                snake_tail.SetType(Piece.Type.FILL).Draw();
        }

        public bool hasColision(Piece.Type type)
        {
            bool result = false;
            Piece next_position = board.NextPiece(direction, GetHead());

            if (next_position.GetType().Equals(type))
                result = true;

            return result;
        }

        public void Refresh()
        {
            // Cogemos la dirección actual pulsada por el jugador en el teclado
            direction = board.GetDirectionKeyboard();

            // Por defecto increase_snake = false
            bool increase_snake = false;

            // Obtenemos la siguiente posición a la cabeza actual, antes de mover para realizar comprobaciones
            Piece next_position = board.NextPiece(direction, GetHead());

            // Si el snake se encuentra con otra posición de tipo BODY, terminará el juego
            if (hasColision(Piece.Type.BODY))
                throw new GameOverException(board);

            // Si el tipo de la pieza actual es FOOD significa que ha encontrado comida
            if (hasColision(Piece.Type.NORMAL_FOOD) || hasColision(Piece.Type.SPECIAL_FOOD))
            {
                // Seteamos increase_snake a true, informando que hay que incrementar el snake.body
                increase_snake = true;

                // Aumentamos puntuación del usuario en 1
                board.RefreshScore(1);

                if (hasColision(Piece.Type.NORMAL_FOOD))
                    board.GetFood().SetActiveFood(false); // Seteamos active_food = false, informando que ya no existe comida en el tablero
                else if (hasColision(Piece.Type.SPECIAL_FOOD))
                    board.GetSpecialFood().SetActiveFood(false); // Seteamos active_food = false, informando que ya no existe comida en el tablero
            }

            // Movemos el snake a la siguiente posición
            MoveSnake(increase_snake);

            // Comprobamos si existe o debemos asignar comida en el tablero
            board.GetFood().UpdateFood(board);

            // Comida especial
            board.GetSpecialFood().UpdateFood(board);
        }
    }
}