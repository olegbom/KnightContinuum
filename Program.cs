// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

Stopwatch sw = Stopwatch.StartNew();
Board.Width = 6;
Board.Height = 5;
var board = new Board(0,0,1);

List<Board> oldGeneration = new List<Board>();
List<Board> newGeneration = new List<Board>();
oldGeneration.Add(board);

for (int i = 0, max = Board.Height* Board.Width - 1; i < max; i++)
{
    newGeneration.Clear();
    foreach (var b in oldGeneration)
    {
        newGeneration.AddRange(b.GenerateChildrens());
    }
    oldGeneration.Clear();
    oldGeneration.AddRange(newGeneration);
}
newGeneration.Clear();
foreach (var b in oldGeneration)
{
    b.ClearOne();
    newGeneration.AddRange(b.GenerateChildrens());
}




for (var i = 0; i < newGeneration.Count; i++)
{
    var b = newGeneration[i];
    Console.WriteLine($"_________________");
    Console.WriteLine($"{i}:");
    Console.WriteLine(b);
}
Console.WriteLine($"Number of solutions: {newGeneration.Count}");
Console.WriteLine($"Number of generations: {Board.NumberOfGeneration}");
GC.Collect();
sw.Stop();
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds/1000.0:F3} s");
Console.ReadKey();


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

    public Board(Board parent, byte x, byte y, byte level)
    {
        NumberOfGeneration++;
        _parent = parent;
        X = x;
        Y = y;
        Level = level;

    }

    public List<Board> GenerateChildrens()
    {
        var result = new List<Board>(8);

        void TryAddBoard(int dx, int dy)
        {
            int x = X + dx;
            int y = Y + dy;

            if (x >= 0 && y >= 0 && x < Width && y < Height && this[x,y] == 0)
            {
                result.Add(new Board(this, (byte)x, (byte)y, (byte)(Level + 1)));
            }

        }

        TryAddBoard(1, 2);
        TryAddBoard(2, 1);
        TryAddBoard(-1, 2);
        TryAddBoard(-2, 1);
        TryAddBoard(1, -2);
        TryAddBoard(2, -1);
        TryAddBoard(-1, -2);
        TryAddBoard(-2, -1);
        return result;
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
        
      /*  for (int i = 0; i < _board.Length; i++)
        {
            if (_board[i] == 1) 
                _board[i] = 0;
        }*/
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

