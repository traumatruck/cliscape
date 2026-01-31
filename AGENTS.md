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
  - **Never nest multiple classes** within a single file. Each class must be in its own file.
  - For command settings classes, create a separate file (e.g., `EquipCommandSettings.cs` for `EquipCommand.Settings`)
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

## Item Implementation

- **Location**: `src/CliScape.Content/Items/`
- **Registry**: `ItemIds.cs` defines all item IDs; `ItemRegistry.cs` provides lookup by ID or name
- **Base Classes**: `Item` for non-equippable items, `EquippableItem` for equipment
- **Categories**: Equipment organized by tier (BronzeEquipment, IronEquipment, etc.) in `Items/Equippables/`
- **Required Properties** (from `IItem`):
  - `Id` — Use `ItemId` wrapper record
  - `Name` — Use `ItemName` wrapper record
  - `ExamineText` — Classic OSRS-style examine description
  - `BaseValue` — Base value in gold pieces for shop pricing
  - `IsStackable` — Whether item stacks (coins, runes, arrows)
  - `IsTradeable` — Whether item can be sold to shops
- **Equipment Properties** (from `IEquippable`):
  - `Slot` — `EquipmentSlot` enum (Head, Body, Weapon, etc.)
  - `Stats` — `EquipmentStats` with attack/defence/strength bonuses
  - `RequiredAttackLevel`, `RequiredDefenceLevel`, etc.
- **Item Registration**: Each item class must have an `All` array property for `ItemRegistry` auto-registration
- **Data Source**: OSRS Wiki for accurate stats

## Item Actions

Items support a composable action system via `IItemAction` interface.

- **Action Interface**: `IItemAction` in `CliScape.Core.Items` represents an executable action
  - `ActionType` — The `ItemAction` enum value (Examine, Use, Eat, Bury, Drink, Read, Equip)
  - `Description` — Human-readable description of the action
  - `ConsumesItem` — Whether executing the action removes the item from inventory
  - `Execute(item, player)` — Performs the action and returns a result message
- **Built-in Actions** (in `CliScape.Core.Items.Actions`):
  - `ExamineAction` — Shows the item's examine text (singleton, included by default)
  - `UseAction` — Generic use with custom logic (default returns "Nothing interesting happens")
  - `EatAction` — Heals the player, consumes the item
  - `BuryAction` — Grants prayer XP, consumes the item
  - `DrinkAction` — For potions with custom effects
  - `ReadAction` — Displays text content
- **Actionable Items**:
  - `IActionableItem` — Interface with `Actions`, `AvailableActions`, `SupportsAction()`, `GetAction()`
  - `ActionableItem` — Base class, auto-includes `ExamineAction`; use `WithAction()` for composition
  - `EdibleItem` — Extends `ActionableItem`, auto-adds `EatAction` and `UseAction`
  - `BuryableItem` — Extends `ActionableItem`, auto-adds `BuryAction` and `UseAction`
- **Adding Multiple Actions**: Use `WithAction()` or `WithActions()` to add actions to any `ActionableItem`
- **CLI Usage**: `item <name> --examine|--use|--eat|--bury|--drink|--read|--equip`
- **Example**: See `src/CliScape.Content/Items/Food.cs` for edible items

## Shop Implementation

- **Location**: `src/CliScape.Content/Shops/`
- **Pattern**: Static class per location (e.g., `LumbridgeShops`, `VarrockShops`)
- **Shop Properties**:
  - `Name` — Use `ShopName` wrapper record
  - `IsGeneralStore` — General stores buy any tradeable item
  - `SellPriceMultiplier` — Price multiplier when selling to players
  - `BuyPriceMultiplier` — Price multiplier when buying from players
- **Stock**: Use `shop.AddStock(item, quantity)` to populate inventory
- **Pricing**: OSRS-style dynamic pricing based on stock levels
- **Location Integration**: Locations have `IReadOnlyList<Shop> Shops` property
- **Example**: See `src/CliScape.Content/Shops/LumbridgeShops.cs`

## Inventory & Equipment

- **Inventory**: 28 slots matching OSRS, supports stackable items
- **Equipment**: 11 slots (Head, Cape, Neck, Ammo, Weapon, Body, Shield, Legs, Hands, Feet, Ring)
- **Player Properties**: `player.Inventory`, `player.Equipment`
- **Gold**: Gold is stored as Coins item in the inventory, not as a separate property
- **Combat Integration**: Equipment bonuses automatically applied via `CombatCalculator`

## Wrapper Records

The codebase uses wrapper records for type safety:
- `NpcName` — Wraps NPC display names
- `LocationName` — Wraps location identifiers
- `SkillName` — Wraps skill identifiers
- `ItemId` — Wraps item type identifiers
- `ItemName` — Wraps item display names
- `ShopName` — Wraps shop display names

Always use the appropriate wrapper rather than raw strings.

## Testing

- **Framework**: xUnit in `tests/CliScape.Content.Tests`
- **Naming**: Test classes use `*Tests` suffix
- **Coverage**: `coverlet.collector` available for coverage reports

## Git Conventions

- **Commits**: Short imperative messages (e.g., "Add goblin NPC", "Fix combat calculation")
- **PRs**: Describe change, list test commands run, note gameplay impacts
- **Screenshots**: Include CLI output for UI/UX changes
