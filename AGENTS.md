# Repository Guidelines

CliScape is an OSRS-inspired CLI game built with .NET 10. This document provides context for AI agents working on the codebase.

**Keep this file current.** When making changes that affect project structure, conventions, or patterns documented here, update this file as part of the same change.

## Project Structure

```
src/
├── CliScape.Cli        → CLI entry point, command wiring (Spectre.Console.Cli)
├── CliScape.Game       → Game orchestration, state management, persistence
├── CliScape.Core       → Domain models: players, NPCs, skills, combat mechanics
├── CliScape.Content    → Game content: locations, NPCs, items
└── CliScape.Infrastructure → Configuration, binary persistence implementation
tests/
└── CliScape.Content.Tests → xUnit tests for content/core behavior
tools/
└── CliScape.MapVisualizer → Map visualization utility
```

### Project Dependencies
```
Cli → Game → Content → Core
         ↘ Infrastructure ↗
```

### Key Dependencies
- **Spectre.Console** — CLI framework and terminal UI
- **OneOf** — Discriminated unions for result types
- **xUnit + coverlet.collector** — Testing and coverage

## Build & Run

```bash
dotnet build CliScape.slnx          # Build full solution
dotnet run --project src/CliScape.Cli   # Run CLI directly
./cliscape.sh                       # Convenience wrapper from repo root
dotnet test tests/CliScape.Content.Tests  # Run tests
```

## Coding Conventions

- **Indentation**: 4 spaces
- **Naming**: PascalCase for types/methods/properties; camelCase for locals/parameters
- **Files**: One public type per file; filename matches type name
- **Nullability**: Nullable reference types enabled; avoid `null`, use `Option` or sensible defaults
- **IDE**: `.sln.DotSettings` provides Rider/ReSharper hints

## CLI Command Implementation

Commands use Spectre.Console.Cli framework.

- **Location**: `src/CliScape.Cli/Commands/` organized by feature (Combat/, Save/, Status/, Walk/)
- **Interface**: Commands implement `ICommand` (project-specific marker with `IsEnabled` property)
- **Base Classes**:
  - `Command<TSettings>` for commands with settings
  - `Command` for commands without settings
- **Settings**: Inherit from `CommandSettings`; use `[CommandArgument]` or `[CommandOption]` attributes
- **Return Values**: Use `ExitCode.Success` (0) or `ExitCode.Failure` (1)
- **GameState Access**: Use `GameState.Instance` singleton for player/location access
- **Registration**: Commands registered in `Program.cs` via `config.AddCommand<T>()`

## GameState & Persistence

- **Singleton**: `GameState.Instance` is the central state manager
- **Initialization**: Call `GameState.Instance.Configure(persistenceStore)` before use
- **Player Access**: `GameState.Instance.Player` returns current player
- **Location Access**: `GameState.Instance.FindLocation(name)` to look up locations
- **Combat**: `GameState.Instance.StartCombat(npc)` and `CombatSession` methods
- **Save Format**: Binary via `BinaryPersistenceStore`
- **Save Location**: `~/.local/share/CliScape/save.bin`

## Location Implementation

- **Location**: `src/CliScape.Content/Locations/` (subfolders like `Towns/`)
- **Interface**: Implement `ILocation` from `CliScape.Core.World`
- **Static Name**: Define `public static LocationName Name { get; } = new("Location Name");`
- **Required Properties**:
  - `Shop` — Optional shop at location (can be `null`)
  - `Bank` — Optional bank at location (can be `null`)
  - `AvailableNpcs` — Collection of `ICombatableNpc` at this location
- **Discovery**: Locations are auto-discovered via reflection (no manual registration)

## NPC Implementation

- **Location**: `src/CliScape.Content/Npcs/`
- **Base Class**: `CombatableNpc` from `CliScape.Core.Npcs`
- **Pattern**: Singleton with private constructor:
  ```csharp
  public sealed class Goblin : CombatableNpc
  {
      public static readonly Goblin Instance = new();
      private Goblin() { /* set properties */ }
  }
  ```
- **Required Properties** (from `ICombatableNpc`):
  - `Name` — Use `new NpcName("Name")` wrapper record
  - `CombatLevel`, `Hitpoints`, `AttackSpeed`
  - `IsAggressive`, `IsPoisonous`
  - `AttackLevel`, `StrengthLevel`, `DefenceLevel`, `RangedLevel`, `MagicLevel`
  - Attack bonuses: `StabAttackBonus`, `SlashAttackBonus`, `CrushAttackBonus`, `RangedAttackBonus`, `MagicAttackBonus`
  - Defence bonuses: `StabDefenceBonus`, `SlashDefenceBonus`, `CrushDefenceBonus`, `RangedDefenceBonus`, `MagicDefenceBonus`
  - Strength: `MeleeStrengthBonus`, `RangedStrengthBonus`, `MagicDamageBonus`, `PrayerBonus`
  - `Attacks` — Array of `NpcAttack` records
  - `Immunities` — Use `NpcImmunities.None` for no immunities
  - `SlayerLevel`, `SlayerXp`, `SlayerCategory`
- **Optional**: `ElementalWeakness` — Use `ElementalWeakness.None` when not applicable
- **Data Source**: OSRS Wiki for accurate stats
- **Example**: See `src/CliScape.Content/Npcs/Cow.cs`

## Wrapper Records

The codebase uses wrapper records for type safety:
- `NpcName` — Wraps NPC display names
- `LocationName` — Wraps location identifiers
- `SkillName` — Wraps skill identifiers

Always use the appropriate wrapper rather than raw strings.

## Testing

- **Framework**: xUnit in `tests/CliScape.Content.Tests`
- **Naming**: Test classes use `*Tests` suffix
- **Coverage**: `coverlet.collector` available for coverage reports

## Git Conventions

- **Commits**: Short imperative messages (e.g., "Add goblin NPC", "Fix combat calculation")
- **PRs**: Describe change, list test commands run, note gameplay impacts
- **Screenshots**: Include CLI output for UI/UX changes
