namespace CommandPattern.Core.Commands;

// Command-rollen: en anmodning gjort til et objekt
public interface ICommand
{
    string Name { get; }   // til log og visning
    void Execute();
    void Undo();
}