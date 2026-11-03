namespace CommandPattern.Core.Game;

public sealed class Player
{
    public const int JumpHeight = 4;
    private readonly Level _level;

    public Player(Level level)
    {
        _level = level;
        Position = level.Start;
        Fall();
    }

    public Position Position { get; private set; }
    public int Height => _level.Height - 2 - Position.Y;   // 0 = står på gulvet
    public bool HasWon => Position.Y <= _level.GoalRow;

    public void Walk(int dx)
    {
        int newX = Math.Clamp(Position.X + dx, 0, _level.Width - 1);
        Position = Position with  {X = newX};
        Fall();
    }

    public void Jump()
    {
        int peak = Math.Max(0, Position.Y - JumpHeight);
        Position = Position with { Y = peak };
        Fall();
    }

    // flyt direkte uden tyngdekraft
    // bruges senere af Undo og Replay
    public void MoveTo(Position position) => Position = position;

    private void Fall()
    {
        int y = Position.Y;
        while (y + 1 < _level.Height && !_level.IsPlatform(Position.X, y + 1))
            y++;
        Position = Position with { Y = y };
    }
}