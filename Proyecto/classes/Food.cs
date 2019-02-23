using System;
namespace Proyecto.classes
{
    public abstract class Food
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

        public abstract Food UpdateFood(Board board);
    }
}