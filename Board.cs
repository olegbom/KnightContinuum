using System.Numerics;
using System.Text;
using Raylib_CsLo;

namespace KnightContinuum;

public class Board
{
    public static long NumberOfGeneration { get; private set; } = 0;
    public static int Width;
    public static int Height;

    private byte this[int i, int j]
    {
        get
        {
            if (_parent == null)
                return 0;
            if (i == _parent.X && j == _parent.Y)
                return _parent.Level;
            return _parent[i, j];
        }
    }

    private Board _parent = null;
    public byte X;
    public byte Y;
    public byte Level;

    public Board(byte x, byte y, byte level)
    {
        NumberOfGeneration++;
        X = x;
        Y = y;
        Level = level;
    }

    public Board(Board parent, byte x, byte y, byte level) :this(x,y,level)
    {
        _parent = parent;
    }

    private readonly (int dx, int dy)[] _turns =
    {
        (1, 2),
        (2, 1),
        (-1, 2),
        (-2, 1),
        (1, -2),
        (2, -1),
        (-1, -2),
        (-2, -1),
    };

    public IEnumerable<Board> GenerateChildren()
    {
        if (Level == Width * Height)
        {
            for (int i = 0; i < _turns.Length; i++)
            {
                int x = X + _turns[i].dx;
                int y = Y + _turns[i].dy;
                if (x >= 0 && y >= 0 && x < Width && y < Height && this[x, y] == 1)
                {
                    yield return new Board(this, (byte)x, (byte)y, 1);
                    break;
                }
            }
            yield break;
        }


        IEnumerable<Board> TryAddBoard(int dx, int dy)
        {
            int x = X + dx;
            int y = Y + dy;

            if (x >= 0 && y >= 0 && x < Width && y < Height && this[x, y] == 0)
            {
                var b = new Board(this, (byte)x, (byte)y, (byte)(Level + 1));
                return b.GenerateChildren();
            }
            return Enumerable.Empty<Board>();
        }

        if (Level > 3)
        {
            for(int i = 0; i < _turns.Length; i++)
                foreach (var c in TryAddBoard(_turns[i].dx, _turns[i].dy))
                    yield return c;
           
        }
        else
        {

            List<Board>[] listsOfBoards = new List<Board>[_turns.Length];
            Parallel.For(0, _turns.Length, (i) =>
            {
                listsOfBoards[i] = TryAddBoard(_turns[i].dx, _turns[i].dy).ToList();
            });
            foreach (var list in listsOfBoards)
            {
                foreach (var board in list)
                {
                    yield return board;
                }
            }
        }
    }

    public void Draw()
    {
        if (_parent != null)
        {
            Raylib.DrawLineEx( new Vector2(100 + X * 100, 100 + Y * 100),
                new Vector2(100 + _parent.X * 100, 100 + _parent.Y * 100),4, Raylib.BLACK);
            _parent.Draw();
        }
    }

    public void DrawLabel()
    {
        if (_parent != null)
        {
            var rect = new Rectangle(100 + X * 100 - 25, 100 + Y * 100 - 25, 54, 50);
         

            Raylib.DrawRectangleRec(new Rectangle(100 + X * 100 - 25, 100 + Y * 100 - 25, 54, 50),
                new Color(194, 143, 75, 255));  
            Raylib.DrawRectangleLinesEx(rect, 2, Raylib.BLACK);
            Raylib.DrawText($"{Level}", 100 + X * 100 - 20, 100 + Y * 100 - 20, 40, Raylib.BLACK);
            _parent.DrawLabel();
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width - 1; i++)
            {
                sb.Append($"{this[i, j],3} ");
            }
            sb.AppendLine($"{this[Width - 1, j],3}");
        }

        return sb.ToString();

    }
}