using System.Numerics;
using System.Text;
using Raylib_CsLo;

public class Board
{
    public static long NumberOfGeneration { get; private set; } = 0;
    public static int Width;
    public static int Height;
    private static byte[,] _mapHelper;

    public static void UpdateMap() => _mapHelper = new byte[Width, Height];

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

    public Board(Board parent, byte x, byte y, byte level)
    {
        NumberOfGeneration++;
        _parent = parent;
        X = x;
        Y = y;
        Level = level;
    }

    public IEnumerable<Board> GenerateChildren()
    {
        _mapHelper[X, Y] = Level;
        if (Level == Width * Height)
        {
            IEnumerable<Board> FindLoop(int dx, int dy)
            {
                int x = X + dx;
                int y = Y + dy;
                if (x >= 0 && y >= 0 && x < Width && y < Height && this[x, y] == 1)
                    yield return new Board(this, (byte)x, (byte)y, 1);
            }
            foreach (var c in FindLoop(1, 2)) yield return c;
            foreach (var c in FindLoop(2, 1)) yield return c;
            foreach (var c in FindLoop(-1, 2)) yield return c;
            foreach (var c in FindLoop(-2, 1)) yield return c;
            foreach (var c in FindLoop(1, -2)) yield return c;
            foreach (var c in FindLoop(2, -1)) yield return c;
            foreach (var c in FindLoop(-1, -2)) yield return c;
            foreach (var c in FindLoop(-2, -1)) yield return c;
            yield break;
        }


        IEnumerable<Board> TryAddBoard(int dx, int dy)
        {
            int x = X + dx;
            int y = Y + dy;

            if (x >= 0 && y >= 0 && x < Width && y < Height && this[x,y] == 0)
            {
                var b = new Board(this, (byte)x, (byte)y, (byte)(Level + 1));
                foreach (var c in b.GenerateChildren())
                    yield return c;
            }
        }

        foreach (var c in TryAddBoard(1, 2)) yield return c; 
        foreach (var c in TryAddBoard(2, 1)) yield return c; 
        foreach (var c in TryAddBoard(-1, 2)) yield return c; 
        foreach (var c in TryAddBoard(-2, 1)) yield return c; 
        foreach (var c in TryAddBoard(1, -2)) yield return c; 
        foreach (var c in TryAddBoard(2, -1)) yield return c; 
        foreach (var c in TryAddBoard(-1, -2)) yield return c; 
        foreach (var c in TryAddBoard(-2, -1)) yield return c;
        _mapHelper[X, Y] = 0;
    }

    public void ClearOne()
    {
        if (_parent == null)
        {
            Level = 0;
        }
        else
        {
            _parent.ClearOne();
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