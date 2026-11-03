namespace CommandPattern.Core.Input;

// Oversætter taster til handlinger. Fordi tasten peger på en handling
// og ikke på kode, kan tasterne omlægges mens spillet kører.
public sealed class InputHandler
{
    private readonly Dictionary<ConsoleKey, GameAction> _bindings = new()
    {
        [ConsoleKey.A] = GameAction.MoveLeft,
        [ConsoleKey.LeftArrow] = GameAction.MoveLeft,
        [ConsoleKey.D] = GameAction.MoveRight,
        [ConsoleKey.RightArrow] = GameAction.MoveRight,
        [ConsoleKey.W] = GameAction.Jump,
        [ConsoleKey.Spacebar] = GameAction.Jump,
        [ConsoleKey.Q] = GameAction.JumpLeft,
        [ConsoleKey.E] = GameAction.JumpRight,
        [ConsoleKey.Z] = GameAction.Undo,
        [ConsoleKey.Y] = GameAction.Redo,
        [ConsoleKey.R] = GameAction.Replay,
        [ConsoleKey.K] = GameAction.Remap,
        [ConsoleKey.Escape] = GameAction.Quit,
    };

    public bool TryGetAction(ConsoleKey key, out GameAction action) =>
        _bindings.TryGetValue(key, out action);

    public void Bind(ConsoleKey key, GameAction action) => _bindings[key] = action;

    public IEnumerable<ConsoleKey> KeysFor(GameAction action) =>
        _bindings.Where(b => b.Value == action).Select(b => b.Key);
}