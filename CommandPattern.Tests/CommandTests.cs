using CommandPattern.Core;
using CommandPattern.Core.Commands;
using CommandPattern.Core.Game;
using CommandPattern.Core.Input;

namespace CommandPattern.Tests;

public class CommandManagerTests
{
    [Fact]
    public void Undo_restores_position_even_after_a_fall()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        manager.Execute(new JumpCommand(player));      // (1,16)
        manager.Execute(new MoveLeftCommand(player));  // falder til (0,19)

        manager.Undo();

        Assert.Equal(new Position(1, 16), player.Position);
    }

    [Fact]
    public void New_command_after_undo_clears_redo_stack()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        manager.Execute(new MoveRightCommand(player));
        manager.Undo();

        manager.Execute(new JumpCommand(player));

        Assert.False(manager.CanRedo);
    }

    [Fact]
    public void Redo_reapplies_an_undone_command()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        manager.Execute(new MoveRightCommand(player));  // (2,19)
        manager.Undo();                                 // (1,19)

        bool redone = manager.Redo();

        Assert.True(redone);
        Assert.Equal(new Position(2, 19), player.Position);
    }

    [Fact]
    public void Undo_on_empty_history_returns_false()
    {
        var manager = new CommandManager();

        Assert.False(manager.Undo());
    }

    [Fact]
    public void Undo_all_returns_to_start()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        var start = player.Position;
        manager.Execute(new MoveRightCommand(player));
        manager.Execute(new JumpCommand(player));
        manager.Execute(new MoveLeftCommand(player));

        while (manager.CanUndo)
            manager.Undo();

        Assert.Equal(start, player.Position);
    }

    [Fact]
    public void Log_records_execute_undo_and_redo()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();

        manager.Execute(new MoveRightCommand(player));
        manager.Undo();
        manager.Redo();

        Assert.Collection(manager.Log,
            line => Assert.StartsWith("Udført:", line),
            line => Assert.StartsWith("Fortrudt:", line),
            line => Assert.StartsWith("Gentaget:", line));
    }

    [Fact]
    public void History_excludes_undone_commands_and_keeps_order()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        var first = new MoveRightCommand(player);
        var second = new JumpCommand(player);
        var third = new MoveLeftCommand(player);
        manager.Execute(first);
        manager.Execute(second);
        manager.Execute(third);

        manager.Undo();

        Assert.Equal(new ICommand[] { first, second }, manager.History);
    }
}

public class MacroCommandTests
{
    [Fact]
    public void Macro_undo_reverts_everything_as_one_step()
    {
        var player = new Player(Level.CreateDefault());
        var manager = new CommandManager();
        var start = player.Position;

        manager.Execute(new MacroCommand("Hop til højre",
            new JumpCommand(player), new MoveRightCommand(player)));
        manager.Undo();

        Assert.Equal(start, player.Position);
        Assert.Equal(0, manager.UndoCount);
    }

    [Fact]
    public void Empty_macro_is_rejected()
    {
        Assert.Throws<ArgumentException>(() => new MacroCommand("Tom"));
    }
}

public class GameSessionTests
{
    private static readonly GameAction[] RouteToTop =
    {
        GameAction.MoveRight, GameAction.MoveRight, GameAction.MoveRight,
        GameAction.Jump, GameAction.Jump,
        GameAction.MoveRight, GameAction.MoveRight, GameAction.MoveRight, GameAction.MoveRight,
        GameAction.Jump, GameAction.Jump,
        GameAction.MoveLeft, GameAction.MoveLeft, GameAction.MoveLeft, GameAction.MoveLeft,
        GameAction.Jump, GameAction.Jump,
    };

    [Fact]
    public void Replay_reproduces_the_same_end_position()
    {
        var session = new GameSession();
        foreach (var action in RouteToTop.Take(9))
            session.Perform(action);
        session.Perform(GameAction.Undo);
        var end = session.Player.Position;

        session.Replay();

        Assert.Equal(end, session.Player.Position);
    }

    [Fact]
    public void Rebinding_changes_which_action_a_key_triggers()
    {
        var input = new InputHandler();

        input.Bind(ConsoleKey.A, GameAction.Jump);

        Assert.True(input.TryGetAction(ConsoleKey.A, out var action));
        Assert.Equal(GameAction.Jump, action);
    }

    [Fact]
    public void The_default_level_can_be_completed()
    {
        var session = new GameSession();

        foreach (var action in RouteToTop)
            session.Perform(action);

        Assert.True(session.Player.HasWon);
    }
}