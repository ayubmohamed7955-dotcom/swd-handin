using CommandPattern.Core.Game;

namespace CommandPattern.Core.Commands;

public static class ReplayPlayer
{
    // afterEach er en callback, så Core ikke behøver at kende konsollen
    public static void Replay(Player player, Position start,
                              IEnumerable<ICommand> history,
                              Action<ICommand, int>? afterEach = null)
    {
        player.MoveTo(start);
        int step = 0;
        foreach (var command in history)
        {
            command.Execute();
            afterEach?.Invoke(command, ++step);
        }
    }
}