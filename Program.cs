// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using KnightContinuum;
using Raylib_CsLo;

Stopwatch sw = Stopwatch.StartNew();
Board.Width = 10;
Board.Height = 3;

var board = new Board(0,0,1);

List<Board> newGeneration = board.GenerateChildren().ToList();

sw.Stop();

Raylib.InitWindow(1520, 720, "Knight Continuum!");
Raylib.SetTargetFPS(60);

int index = 0;
while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib.BEIGE);
    Raylib.DrawFPS(10, 10);
    Raylib.DrawText($"{index + 1}/{newGeneration.Count}", 150, 10, 40, Raylib.BLACK);
    Raylib.DrawText($"Elapsed: {sw.ElapsedMilliseconds / 1000.0:F3} s", 300, 10, 40, Raylib.BLACK);
    if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
    {
        index = Math.Min(index + 1, newGeneration.Count - 1);
    }

    if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
    {
        index = Math.Max(index - 1, 0);
    }

    for (int i = 0; i <= Board.Width; i++)
    {
        Raylib.DrawLineEx(new Vector2(50 + i*100, 50),
            new Vector2(50 + i * 100, 50 + Board.Height* 100), 4, Raylib.BLUE);

    }

    for (int i = 0; i <= Board.Height; i++)
    {
        Raylib.DrawLineEx(new Vector2(50, 50 + i * 100),
            new Vector2(50 + Board.Width * 100, 50 + i * 100), 4, Raylib.BLUE);

    }

    if (newGeneration.Count > index && index >= 0)
    {
        newGeneration[index].Draw();
        newGeneration[index].DrawLabel();
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

