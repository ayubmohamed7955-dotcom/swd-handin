# Command Pattern â€“ klatrespil i C#

Afleveringsprojekt til SW4SWD (Design Patterns). Et lille klatrespil i konsollen, hvor hver tast bliver til et kommando-objekt. Det viser **Command-patternet** med undo/redo, makro-kommandoer, replay og omlÃ¦gning af taster.

**Gruppe:** 7

| Navn | auID |
| --- | --- |
| Ayub Mohamed Yusuf | au781514 |
| Mohamad Saleh Yassine | au759451 |
| Mohamed Abdirahman Hassan Mahamod | au758453 |

## KÃ¸r projektet

KrÃ¦ver **.NET 10 SDK** (Visual Studio 2026 eller nyere).

```bash
dotnet run --project CommandPattern.Game   # start spillet
dotnet test                                # kÃ¸r alle tests
```

I Visual Studio: Ã¥bn `CommandPattern.sln`, sÃ¦t `CommandPattern.Game` som startup-projekt, og tryk F5.

## Styring

| Tast | Handling |
| --- | --- |
| A / â† | GÃ¥ venstre |
| D / â†’ | GÃ¥ hÃ¸jre |
| W / Space | Hop |
| Q / E | Makro: hop til venstre / hÃ¸jre |
| Z / Y | Undo / redo |
| R | Replay af historikken |
| K | OmlÃ¦g en tast |
| Esc | Afslut |

MÃ¥let er at nÃ¥ toppen (â–¶). Rute: D D D, W, W, D D D D, W, W, A A A A, W, W.

## Projektstruktur

```
CommandPattern.Core    Patternet og spillogikken (kender ikke konsollen)
  Commands/            ICommand, PlayerCommand, konkrete kommandoer,
                       CommandManager, MacroCommand, ReplayPlayer
  Game/                Position, Level, Player
  Input/               GameAction, InputHandler
  GameSession.cs
CommandPattern.Game    Konsol-UI (ConsoleRenderer, Program)
CommandPattern.Tests   xUnit-tests
```

## Patternets roller

| GoF-rolle | Klasse | Ansvar |
| --- | --- | --- |
| Command | `ICommand` | FÃ¦lles interface: `Execute()`, `Undo()`, `Name` |
| ConcreteCommand | `MoveLeftCommand`, `MoveRightCommand`, `JumpCommand` | Binder Ã©n handling til receiveren |
| Receiver | `Player` | Ved *hvordan* man gÃ¥r, hopper og falder |
| Invoker | `CommandManager` | UdfÃ¸rer kommandoer og holder undo/redo-stakke |
| Client | `GameSession` | Opretter kommandoerne og giver dem til invokeren |
| Composite command | `MacroCommand` | En kommando bestÃ¥ende af flere kommandoer |

## Vigtige designvalg

- **Undo gemmer tilstand.** Tyngdekraften gÃ¸r handlingerne ikke-reversible, sÃ¥ hver kommando gemmer spillerens position fÃ¸r `Execute()` i stedet for at lave den omvendte operation (`PlayerCommand`, Template Method).
- **Ny kommando-instans pr. handling.** `GameSession` bruger `Func<ICommand>`, fordi hver instans har sin egen gemte position.
- **Makroer fortrydes bagfra** og tÃ¦ller som Ã©t skridt i historikken.
- **Undo/redo er ikke selv kommandoer.** De arbejder pÃ¥ historikken og gÃ¥r direkte til invokeren.
- **Core kender ikke konsollen.** Replay bruger en callback, sÃ¥ UI'et selv bestemmer, hvordan hvert trin vises.

## Tests

12 xUnit-tests dÃ¦kker undo (ogsÃ¥ efter fald), redo, tom historik, log, historikkens rÃ¦kkefÃ¸lge, makro-undo, tom makro, replay, omlÃ¦gning af taster og gennemfÃ¸rsel af banen.

## Arbejdsfordeling

- **Mohamad:** projektopsÃ¦tning, `Position`, `Level`, `Player`, `ICommand`
- **Ayub:** kommandoer, undo/redo, makro, input, `GameSession`, konsol-UI, replay og tests
- **Mohamed:** [udfyld, fx rapportafsnit, UML eller slides]

## Referencer

- Gamma, Helm, Johnson & Vlissides: *Design Patterns: Elements of Reusable Object-Oriented Software* (1994)
- Refactoring.Guru: [Command](https://refactoring.guru/design-patterns/command)
- Robert Nystrom: *Game Programming Patterns*, kapitlet [Command](https://gameprogrammingpatterns.com/command.html)

