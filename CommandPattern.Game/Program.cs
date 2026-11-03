using System.Text;
using CommandPattern.Core;
using CommandPattern.Core.Input;
using CommandPattern.Game;

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;
Console.Clear();

var session = new GameSession();
var renderer = new ConsoleRenderer();
string? status = null;

// UI'et opretter aldrig selv kommandoer og kalder aldrig Player direkte
while (true)
{
    renderer.Draw(session, status);
    status = null;

    var key = Console.ReadKey(intercept: true).Key;
    if (!session.Input.TryGetAction(key, out var action))
        continue;

    switch (action)
    {
        case GameAction.Quit:
            return;
        case GameAction.Replay:
            RunReplay();
            break;
        case GameAction.Remap:
            RunRemap();
            break;
        case GameAction.Undo when !session.Commands.CanUndo:
            status = "Intet at fortryde";
            break;
        default:
            session.Perform(action);
            break;
    }
}

void RunRemap()
{
    renderer.Draw(session, "Hvilken handling? 1=venstre 2=højre 3=hop");
    var target = Console.ReadKey(true).KeyChar switch
    {
        '1' => GameAction.MoveLeft,
        '2' => GameAction.MoveRight,
        '3' => GameAction.Jump,
        _ => (GameAction?)null
    };
    if (target is null) { status = "Annulleret"; return; }

    renderer.Draw(session, $"Tryk ny tast for {target}");
    var newKey = Console.ReadKey(true).Key;
    session.Input.Bind(newKey, target.Value);
    status = $"{newKey} → {target}";
}

void RunReplay()
{
    int total = session.Commands.History.Count;
    if (total == 0) { status = "Ingen historik"; return; }

    session.Replay((command, step) =>
    {
        renderer.Draw(session, $"REPLAY {step}/{total}: {command.Name}");
        Thread.Sleep(350);
    });
    status = "Replay færdig";
}