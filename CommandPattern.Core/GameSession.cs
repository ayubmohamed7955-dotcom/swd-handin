using CommandPattern.Core.Commands;
using CommandPattern.Core.Game;
using CommandPattern.Core.Input;

namespace CommandPattern.Core;

// Client: opretter de konkrete kommandoer, kobler dem til receiveren
// og giver dem til invokeren.
public sealed class GameSession
{
    // Func i stedet for ét objekt pr. handling: hver kommando gemmer sin egen
    // _before-position, så der skal laves en ny instans hver gang.
    private readonly Dictionary<GameAction, Func<ICommand>> _commandFactory;

    public GameSession()
    {
        Level = Level.CreateDefault();
        Player = new Player(Level);
        StartPosition = Player.Position;

        _commandFactory = new()
        {
            [GameAction.MoveLeft]  = () => new MoveLeftCommand(Player),
            [GameAction.MoveRight] = () => new MoveRightCommand(Player),
            [GameAction.Jump]      = () => new JumpCommand(Player),
            [GameAction.JumpRight] = () => new MacroCommand("Hop til højre",
                                         new JumpCommand(Player), new MoveRightCommand(Player)),
            [GameAction.JumpLeft]  = () => new MacroCommand("Hop til venstre",
                                         new JumpCommand(Player), new MoveLeftCommand(Player)),
        };
    }

    public Level Level { get; }
    public Player Player { get; }
    public Position StartPosition { get; }
    public CommandManager Commands { get; } = new();
    public InputHandler Input { get; } = new();

    public bool Perform(GameAction action)
    {
        // Undo/redo er ikke kommandoer: de arbejder på selve historikken
        if (action == GameAction.Undo) return Commands.Undo();
        if (action == GameAction.Redo) return Commands.Redo();

        if (!_commandFactory.TryGetValue(action, out var create))
            return false;

        Commands.Execute(create());
        return true;
    }

    public void Replay(Action<ICommand, int>? afterEach = null) =>
        ReplayPlayer.Replay(Player, StartPosition, Commands.History, afterEach);
}