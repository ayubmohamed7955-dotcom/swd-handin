namespace CommandPattern.Core.Game;

public sealed class Level
{
    private readonly bool[,] _platforms;

    public Level(int width, int height)
    {
        Width = width;
        Height = height;
        _platforms = new bool[width, height];
        AddPlatform(Height - 1, 0, Width - 1); // gulvet
    }

    public int Width {get;}
    public int Height {get;}
    public int GoalRow {get; init;} = 1;
    public Position Start {get; init;}

    public bool IsPlatform(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height && _platforms[x, y];

    public void AddPlatform(int row, int fromX, int toX)
    {
        for (int x = Math.Max(0, fromX); x <= Math.Min(Width - 1, toX); x++)
            _platforms[x, row] = true;
    }

    public static Level CreateDefault()
    {
        var level = new Level(16, 21) {Start = new Position(1, 19), GoalRow = 1};
        level.AddPlatform(17, 1, 5);
        level.AddPlatform(14, 4, 9);
        level.AddPlatform(11, 8, 13);
        level.AddPlatform(8, 3, 9);
        level.AddPlatform(5, 1, 4);
        level.AddPlatform(2, 3, 10);
        return level;
    }
}