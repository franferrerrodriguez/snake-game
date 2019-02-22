using System;
namespace Proyecto.classes
{
    public class Food
    {
        public int x;
        public int y;
        private bool active_food;
        private DateTime time;
        private int time_quit = 5;
        private int time_refresh = 10;

        public Food()
        {
            time = DateTime.Now;
        }

        public bool IsActiveFood()
        {
            return active_food;
        }

        public Food SetActiveFood(bool active_food)
        {
            this.active_food = active_food;
            return this;
        }

        public DateTime GetTime()
        {
            return time;
        }

        public Food SetTime(DateTime time)
        {
            this.time = time;
            return this;
        }

        public int GetTimeQuit()
        {
            return time_quit;
        }

        public Food SetTimeQuit(int time_quit)
        {
            this.time_quit = time_quit;
            return this;
        }

        public int GetTimeRefresh()
        {
            return time_refresh;
        }

        public Food SetTimeRefresh(int time_refresh)
        {
            this.time_refresh = time_refresh;
            return this;
        }

        public Food UpdateFood(Board board)
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

                board.GetBoardPiece(x, y).SetType(Piece.Type.FOOD).Draw();
            }
            return this;
        }

        public Food UpdateSpecialFood(Board board)
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
