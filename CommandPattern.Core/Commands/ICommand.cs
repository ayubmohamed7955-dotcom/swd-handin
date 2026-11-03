namespace CommandPattern.Core.Commands;

public interface ICommand
{
    string Name { get; }   // til log og visning
    void Execute();
}