# Repository Guidelines

## Project Structure & Module Organization
- `src/CliScape.Cli`: CLI entry point and command wiring.
- `src/CliScape.Game`: Game orchestration and runtime flow.
- `src/CliScape.Core`: Core domain models and mechanics.
- `src/CliScape.Content`: Game content (locations, NPCs, data).
- `tests/CliScape.Content.Tests`: xUnit test project for content/core behavior.
- `tools/`: helper scripts or utilities (when present).

## Build, Test, and Development Commands
- `dotnet build CliScape.slnx`: build the full solution.
- `dotnet run --project src/CliScape.Cli`: run the CLI directly.
- `./cliscape.sh`: convenience wrapper to run the CLI from repo root.
- `dotnet test tests/CliScape.Content.Tests`: run the test suite.

## Coding Style & Naming Conventions
- C# 4-space indentation; follow standard .NET conventions.
- Types/methods/properties use PascalCase; locals/parameters use camelCase.
- One public type per file; filenames match the primary type.
- Nullable reference types and implicit usings are enabled; avoid `null` by default.
- `.sln.DotSettings` exists for IDE hints; align with existing patterns.

## Testing Guidelines
- Framework: xUnit (`tests/CliScape.Content.Tests`).
- Add new tests under that project; use `*Tests` class naming.
- Coverage collection is available via `coverlet.collector` if needed.

## NPC Implementation Guidelines
- **Location**: All NPC implementations go in `src/CliScape.Content/Npcs/`.
- **Base Class**: Combatable NPCs inherit from `CombatableNpc` (in `CliScape.Core.Npcs`).
- **One Type Per File**: Each NPC type (e.g., Goblin, Cow) gets its own file.
- **Single Instance**: Each NPC class defines one static `Instance` property with the NPC's stats.
  - Use private constructor to prevent external instantiation.
  - Example: `public static readonly Goblin Instance = new() { ... };`
- **Required Properties**: Must set all required properties from `ICombatableNpc`:
  - Basic info: `Name`, `CombatLevel`, `Hitpoints`, `AttackSpeed`, `IsAggressive`, `IsPoisonous`
  - Stats: `AttackLevel`, `StrengthLevel`, `DefenceLevel`, `RangedLevel`, `MagicLevel`
  - Offensive bonuses: Stab/Slash/Crush/Ranged/Magic attack bonuses
  - Defensive bonuses: Stab/Slash/Crush/Ranged/Magic defence bonuses
  - Strength bonuses: Melee/Ranged/Magic strength/damage bonuses, `PrayerBonus`
  - Combat mechanics: `Attacks` (collection of `NpcAttack`), `Immunities` (NpcImmunities record)
  - Slayer: `SlayerLevel`, `SlayerXp`, `SlayerCategory`
  - Optional: `ElementalWeakness` for magic-vulnerable NPCs
- **Attack Definitions**: Use `NpcAttack` record for each attack style the NPC can perform.
  - Single-attack NPCs: Create array with one `NpcAttack`.
  - Multi-attack NPCs (like bosses): Create multiple `NpcAttack` entries with different styles/max hits.
- **Data Source**: Reference OSRS Wiki for accurate stats, bonuses, and combat mechanics.
- **Example**: See `src/CliScape.Content/Npcs/Cow.cs` for a complete implementation.

## Commit & Pull Request Guidelines
- Commit messages in history are short, imperative statements (e.g., "Add content", "Travel rework").
- Keep commits focused; include context in the body if behavior changes.
- PRs should describe the change, list test commands run, and note gameplay impacts.
- Attach screenshots or sample CLI output when UI/UX changes are involved.
