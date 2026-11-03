using CommandPattern.Core.Game;

namespace CommandPattern.Core.Commands;

// Fælles basisklasse: gemmer spillerens position før Execute, så Undo virker
// selvom tyngdekraften gør handlingerne ikke-reversible (Template Method).
public abstract class PlayerCommand : ICommand
{
    private Position _before;

    protected PlayerCommand(Player player) => Player = player;

    protected Player Player { get; }
    public abstract string Name { get; }

    public void Execute()
    {
        _before = Player.Position;   // gem tilstand
        DoExecute();                 // lad underklassen gøre arbejdet
    }

    public void Undo() => Player.MoveTo(_before);

    protected abstract void DoExecute();
}