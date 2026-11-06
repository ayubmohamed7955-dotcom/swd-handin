# Command Pattern – klatrespil i C#

Afleveringsprojekt til SW4SWD (Design Patterns). Et lille klatrespil i konsollen, hvor hver tast bliver til et kommando-objekt. Det viser **Command-patternet** med undo/redo, makro-kommandoer, replay og omlægning af taster.

**Gruppe:** 7

| Navn | auID |
| --- | --- |
| Ayub Mohamed Yusuf | au781514 |
| Mohamad Saleh Yassine | au759451 |
| Mohamed Abdirahman Hassan Mahamod | au758453 |

## Kør projektet

Kræver **.NET 10 SDK** (Visual Studio 2026 eller nyere).

```bash
dotnet run --project CommandPattern.Game   # start spillet
dotnet test                                # kør alle tests
```

I Visual Studio: åbn `CommandPattern.sln`, sæt `CommandPattern.Game` som startup-projekt, og tryk F5.

## Styring

| Tast | Handling |
| --- | --- |
| A / ← | Gå venstre |
| D / → | Gå højre |
| W / Space | Hop |
| Q / E | Makro: hop til venstre / højre |
| Z / Y | Undo / redo |
| R | Replay af historikken |
| K | Omlæg en tast |
| Esc | Afslut |

Målet er at nå toppen (▶). Rute: D D D, W, W, D D D D, W, W, A A A A, W, W.

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
| Command | `ICommand` | Fælles interface: `Execute()`, `Undo()`, `Name` |
| ConcreteCommand | `MoveLeftCommand`, `MoveRightCommand`, `JumpCommand` | Binder én handling til receiveren |
| Receiver | `Player` | Ved *hvordan* man går, hopper og falder |
| Invoker | `CommandManager` | Udfører kommandoer og holder undo/redo-stakke |
| Client | `GameSession` | Opretter kommandoerne og giver dem til invokeren |
| Composite command | `MacroCommand` | En kommando bestående af flere kommandoer |

## Vigtige designvalg

- **Undo gemmer tilstand.** Tyngdekraften gør handlingerne ikke-reversible, så hver kommando gemmer spillerens position før `Execute()` i stedet for at lave den omvendte operation (`PlayerCommand`, Template Method).
- **Ny kommando-instans pr. handling.** `GameSession` bruger `Func<ICommand>`, fordi hver instans har sin egen gemte position.
- **Makroer fortrydes bagfra** og tæller som ét skridt i historikken.
- **Undo/redo er ikke selv kommandoer.** De arbejder på historikken og går direkte til invokeren.
- **Core kender ikke konsollen.** Replay bruger en callback, så UI'et selv bestemmer, hvordan hvert trin vises.

## Tests

12 xUnit-tests dækker undo (også efter fald), redo, tom historik, log, historikkens rækkefølge, makro-undo, tom makro, replay, omlægning af taster og gennemførsel af banen.

## Arbejdsfordeling

- **Mohamad:** projektopsætning, `Position`, `Level`, `Player`, `ICommand`
- **Ayub:** kommandoer, undo/redo, makro, input, `GameSession`, konsol-UI, replay og tests
- **Mohamed:** [udfyld, fx rapportafsnit, UML eller slides]

## Referencer

- Gamma, Helm, Johnson & Vlissides: *Design Patterns: Elements of Reusable Object-Oriented Software* (1994)
- Refactoring.Guru: [Command](https://refactoring.guru/design-patterns/command)
- Robert Nystrom: *Game Programming Patterns*, kapitlet [Command](https://gameprogrammingpatterns.com/command.html)

