namespace CommandPattern.Core.Commands;

// Invoker: udfører kommandoer og husker dem. Kender kun ICommand.
public sealed class CommandManager
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    private readonly List<string> _log = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;
    public IReadOnlyList<string> Log => _log;

    // De gældende kommandoer i udførelsesrækkefølge (Stack itereres nyeste først)
    public IReadOnlyList<ICommand> History => _undoStack.Reverse().ToList();

    public void Execute(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();          // ny handling gør den gamle "fremtid" ugyldig
        _log.Add($"Udført:  {command.Name}");
    }

    public bool Undo()
    {
        if (!CanUndo) return false;
        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
        _log.Add($"Fortrudt: {command.Name}");
        return true;
    }

    public bool Redo()
    {
        if (!CanRedo) return false;
        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
        _log.Add($"Gentaget: {command.Name}");
        return true;
    }
}