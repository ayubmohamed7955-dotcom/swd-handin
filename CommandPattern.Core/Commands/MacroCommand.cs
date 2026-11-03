namespace CommandPattern.Core.Commands;

// Composite command: en kommando bestående af andre kommandoer
public sealed class MacroCommand : ICommand
{
    private readonly IReadOnlyList<ICommand> _commands;

    public MacroCommand(string name, params ICommand[] commands)
    {
        if (commands.Length == 0)
            throw new ArgumentException("En makro skal indeholde mindst én kommando.");
        Name = name;
        _commands = commands;
    }

    public string Name { get; }

    public void Execute()
    {
        foreach (var command in _commands)
            command.Execute();
    }

    // Fortrydes bagfra: hver delkommando gendanner den position, den selv startede fra
    public void Undo()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
            _commands[i].Undo();
    }
}