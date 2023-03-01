// See https://aka.ms/new-console-template for more information

using System.Text;

var board = new Board(6, 5, new Knight(0,0,1));

List<Board> oldGeneration = new List<Board>();
List<Board> newGeneration = new List<Board>();
oldGeneration.Add(board);

for (int i = 0, max = board.Height*board.Width - 1; i < max; i++)
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
Console.ReadKey();

public readonly struct Knight
{
    public readonly int X;
    public readonly int Y;
    public readonly byte Level;
    public Knight(int x, int y, byte level)
    {
        X = x;
        Y = y;
        Level = level;
    }
}

public class Board
{
    public static long NumberOfGeneration { get; private set; } = 0;

    private byte[] _board;


    public readonly Knight Knight;

    public int Width { get; init; }
    public int Height { get; init; }

    public Board(int width, int height, Knight knight)
    {
        NumberOfGeneration++;
        Width = width;
        Height = height;
        Knight = knight;
        _board = new byte[width * height];
        _board[knight.X + knight.Y * Width] = knight.Level;
    }

    public Board(Board parent, Knight knight)
    {
        NumberOfGeneration++;
        Width = parent.Width;
        Height = parent.Height;
        _board = new byte[parent.Width * parent.Height];
        Array.Copy(parent._board, _board, _board.Length);
        Knight = knight;
        _board[knight.X + knight.Y * Width] = knight.Level;
    }

    public List<Board> GenerateChildrens()
    {
        var result = new List<Board>();

        void TryAddBoard(int dx, int dy)
        {
            int x = Knight.X + dx;
            int y = Knight.Y + dy;

            if (x >= 0 && y >= 0 && x < Width && y < Height && _board[x + y * Width] == 0)
            {
                Knight k = new Knight(x, y, (byte)(Knight.Level + 1));
                result.Add(new Board(this, k));
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
        for (int i = 0; i < _board.Length; i++)
        {
            if (_board[i] == 1) 
                _board[i] = 0;
        }
    }




    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width - 1; i++)
            {
                sb.Append($"{_board[i + j * Width],3} ");
            }
            sb.AppendLine($"{_board[Width - 1 + j * Width],3}");
        }

        return sb.ToString();

    }
}

