using System;
using System.Threading;

namespace Proyecto.classes
{
    /*
     * Clase UpdateFood, encargada del manejo de la comida normal en el juego. Extiende de la clase padre Food
     */
    public class NormalFood : Food
    {
        // Actualiza la comida normal dentro del tablero
        public override Food UpdateFood(Board board)
        {
            if (!IsActiveFood())
            {
                SetActiveFood(true);
                Random rnd;

                bool valid = false;
                do
                {
                    rnd = new Random();
                    x = rnd.Next(0, Board.GetWidth());
                    y = rnd.Next(0, Board.GetHeight());
                    if (board.GetBoardPiece(x, y).GetType().Equals(Piece.Type.FILL))
                        valid = true;
                } while (!valid);

                board.GetBoardPiece(x, y).SetType(Piece.Type.NORMAL_FOOD).Draw();
            }
            return this;
        }

    }
}