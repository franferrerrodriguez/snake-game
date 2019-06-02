using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameProject.snake.classes
{
    /*
     * Clase Punctuation encargada de gestionar las puntuaciones que luego se registrarán en un fichero externo
     */
    public class Punctuation : IComparable<Punctuation>
    {
        private DateTime date;
        private string username;
        private int punctuation;
        private int movements;

        public Punctuation(DateTime date, string username, int punctuation, int movements)
        {
            this.date = date;
            this.username = username;
            this.punctuation = punctuation;
            this.movements = movements;
        }

        public void SetDate(DateTime date)
        {
            this.date = date;
        }

        public DateTime GetDate()
        {
            return date;
        }

        public void SetUsername(string username)
        {
            this.username = username;
        }

        public string GetUsername()
        {
            return username;
        }

        public void SetPunctuation(int punctuation)
        {
            this.punctuation = punctuation;
        }

        public int GetPunctuation()
        {
            return punctuation;
        }

        public void SetMovements(int movements)
        {
            this.movements = movements;
        }

        public int GetMovements()
        {
            return movements;
        }

        public override string ToString()
        {
            return String.Format("{0,20}{1,30}{2,20}{3,20}", date, username, punctuation, movements);
        }

        public int CompareTo(Punctuation punctuation)
        {
            if (GetDate() >= punctuation.GetDate())
                return 1;
            else if (punctuation.GetDate() >= GetDate())
                return -1;
            else
                return 0;
        }
    }
}
