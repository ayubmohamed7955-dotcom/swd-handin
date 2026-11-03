using CommandPattern.Core.Game;

namespace CommandPattern.Core.Commands;

// ConcreteCommands: hver binder én handling til receiveren (Player)

public sealed class MoveLeftCommand(Player player) : PlayerCommand(player)
{
    public override string Name => "Gå venstre";
    protected override void DoExecute() => Player.Walk(-1);
}

public sealed class MoveRightCommand(Player player) : PlayerCommand(player)
{
    public override string Name => "Gå højre";
    protected override void DoExecute() => Player.Walk(+1);
}

public sealed class JumpCommand(Player player) : PlayerCommand(player)
{
    public override string Name => "Hop";
    protected override void DoExecute() => Player.Jump();
}