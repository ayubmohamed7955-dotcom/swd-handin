using System.Text;
using CommandPattern.Core;

namespace CommandPattern.Game;

public sealed class ConsoleRenderer
{
    public void Draw(GameSession session, string? status = null)
    {
        var level = session.Level;
        var pos = session.Player.Position;
        var panel = BuildPanel(session, status);
        var sb = new StringBuilder();

        for (int y = 0; y < level.Height; y++)
        {
            sb.Append(y == level.GoalRow ? "▶|" : " |");
            for (int x = 0; x < level.Width; x++)
            {
                if (x == pos.X && y == pos.Y) sb.Append('@');
                else if (level.IsPlatform(x, y)) sb.Append(y == level.Height - 1 ? '#' : '=');
                else sb.Append(' ');
            }
            sb.Append("|   ");
            string line = y < panel.Count ? panel[y] : "";
            sb.AppendLine(line.PadRight(40));   // PadRight overskriver gammel tekst
        }

        Console.SetCursorPosition(0, 0);        // tegn ovenpå i stedet for Clear() -> intet flimmer
        Console.Write(sb.ToString());
    }

    private static List<string> BuildPanel(GameSession s, string? status)
    {
        var p = new List<string>
        {
            "COMMAND PATTERN – KLATRESPIL",
            $"Højde: {s.Player.Height}  Undo: {s.Commands.UndoCount}  Redo: {s.Commands.RedoCount}",
            status ?? (s.Player.HasWon ? "*** DU NÅEDE TOPPEN! ***" : "Nå toppen ▶"),
            "",
            "A/D gå   W/Space hop   Q/E makro",
            "Z undo   Y redo   R replay",
            "K omlæg tast   Esc afslut",
            "",
            "── Log ──",
        };
        p.AddRange(s.Commands.Log.TakeLast(9));
        return p;
    }
}