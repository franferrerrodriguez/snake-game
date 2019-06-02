using System;

namespace Proyecto.classes
{
    public class SpecialFood : Food
    {
        /*
         * Clase UpdateFood, encargada del manejo de la comida especial en el juego. Extiende de la clase padre Food
         */
        public override Food UpdateFood(Board board)
        {
            // Actualiza la comida especial dentro del tablero, la comida especial aparece y desaparece pasados unos segundos
            if (!IsActiveFood() && GetTime().AddSeconds(GetTimeRefresh()) < DateTime.Now)
            {
                ResetTime();
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

                board.GetBoardPiece(x, y).SetType(Piece.Type.SPECIAL_FOOD).Draw();
            }
            else if (IsActiveFood() && GetTime().AddSeconds(GetTimeQuit()) < DateTime.Now)
            {
                ResetTime();
                SetActiveFood(false);
                board.GetBoardPiece(x, y).SetType(Piece.Type.FILL).Draw();
            }
            return this;
        }

    }
}
