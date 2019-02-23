using System;
namespace Proyecto.classes
{
    public class Piece
    {
        public enum Type { HEAD, BODY, FILL, BORDER, NORMAL_FOOD, SPECIAL_FOOD };
        private BoardCoords boardCoords;
        private CursorCoords cursorCoords;
        private Board.Direction direction;
        private Type type;
        private ConsoleColor color;
        private static bool change_body_color;

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
                case Type.NORMAL_FOOD:
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