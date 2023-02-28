// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices.JavaScript;
using System.Text;

var board = new Board(4, 4, new Knight(0,0,1));
Console.WriteLine(board);

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

    private byte[] _board;


    public readonly Knight Knight;

    public int Width { get; init; }
    public int Height { get; init; }

    public Board(int width, int height, Knight knight)
    {
        Width = width;
        Height = height;
        Knight = knight;
        _board = new byte[width * height];
        _board[knight.X + knight.Y * Width] = knight.Level;
    }

    public Board(Board parent, Knight knight)
    {
        Width = parent.Width;
        Height = parent.Height;
        _board = new byte[parent.Width * parent.Height];
        Array.Copy(parent._board, _board, _board.Length);
        Knight = knight;
        _board[knight.X + knight.Y * Width] = knight.Level;
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

