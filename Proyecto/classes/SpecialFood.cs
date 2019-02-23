using System;
namespace Proyecto.classes
{
    public class SpecialFood : Food
    {
        public SpecialFood()
        {
        }

        public override Food UpdateFood(Board board)
        {
            if (!IsActiveFood() && GetTime().AddSeconds(GetTimeRefresh()) < DateTime.Now)
            {
                SetTime(DateTime.Now);
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
                SetTime(DateTime.Now);
                SetActiveFood(false);
                board.GetBoardPiece(x, y).SetType(Piece.Type.FILL).Draw();
            }
            return this;
        }
    }
}
