# Repository Guidelines

CliScape is an OSRS-inspired CLI game built with .NET 10. This document provides context for AI agents working on the codebase.

**Keep this file current.** When making changes that affect project structure, conventions, or patterns documented here, update this file as part of the same change.

## README.md Maintenance

`README.md` is a player-facing overview of the project. Update it alongside code changes when those changes affect user-visible information (new commands, new skills, new game mechanics, changed build steps, etc.). Follow these rules:

- **Do not add new h2 (`##`) sections** unless the user explicitly requests one. Add content under existing sections or use h3 (`###`) subsections within Game Mechanics.
- **Keep entries concise** — one-line descriptions in tables, short bullet points for mechanics. Match the tone and brevity of existing content.
- **Commands table** — add/remove/rename rows when top-level commands change.
- **Game Mechanics subsections** — update skill lists, combat styles, skilling commands, clue scroll tiers, diary locations, or item/equipment details when the underlying data changes.
- **Project Structure table** — update if projects are added, removed, or renamed.
- Do not rewrite sections unrelated to your change. Only touch what your work affects.

## Project Structure

```
src/
├── CliScape.Cli        → CLI entry point, command wiring (Spectre.Console.Cli)
├── CliScape.Game       → Game orchestration, services, state management, persistence
├── CliScape.Core       → Domain models: players, NPCs, skills, combat, events
├── CliScape.Content    → Game content: locations, NPCs, items
└── CliScape.Infrastructure → Configuration, binary persistence implementation
tests/
├── CliScape.Content.Tests  → Integration tests: persistence, combat engine, drops, events
├── CliScape.Core.Tests     → Unit tests for core domain: combat calc, inventory, equipment, skills
├── CliScape.Game.Tests     → Unit tests for game services: mining, cooking, smelting, slayer, etc.
└── CliScape.Tests.Shared   → Shared test helpers, stubs, and factories
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
- **NSubstitute** — Mocking framework for test doubles

### Architecture Patterns
- **Singleton + DI Ready**: Services expose `Instance` singletons but accept dependencies via constructor for testing
- **Domain Events**: Cross-cutting concerns (level-ups, combat outcomes) use event dispatching
- **Value Objects**: Domain primitives wrapped for type safety (Gold, Damage, Experience, ItemId, etc.)

## Build & Run

```bash
dotnet build CliScape.slnx                # Build full solution
dotnet run --project src/CliScape.Cli     # Run CLI directly
./cliscape.sh                             # Convenience wrapper from repo root
dotnet test CliScape.slnx                 # Run all tests
dotnet test tests/CliScape.Core.Tests     # Run core domain tests only
dotnet test tests/CliScape.Game.Tests     # Run game service tests only
dotnet test tests/CliScape.Content.Tests  # Run content/integration tests only
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

- **Singleton**: `GameState.Instance` is the central state manager implementing `IPlayerManager`, `ILocationRegistry`, `ICombatSessionManager`
- **Initialization**: Call `GameState.Instance.Configure(persistenceStore, savePath)` before use
- **Player Access**: `GameState.Instance.GetPlayer()` returns current player
- **Location Access**: `GameState.Instance.GetLocation(name)` to look up locations
- **Combat**: `GameState.Instance.StartCombat(npc)` and `CombatSession` methods
- **Save Format**: Binary via `BinaryPersistenceStore`
- **Save Location**: `~/.local/share/CliScape/save.bin`

### Game Services

Services in `CliScape.Game` provide orchestrated game logic:

- **CombatEngine** (`Game/Combat/`) — Executes combat turns, processes outcomes, handles XP/drops
- **SlayerService** (`Game/Slayer/`) — Assigns and cancels slayer tasks
- **FishingService** (`Game/Skills/`) — Handles fishing validation and execution
- **WoodcuttingService** (`Game/Skills/`) — Handles tree chopping
- **MiningService** (`Game/Skills/`) — Handles ore mining with pickaxe tier checks
- **SmeltingService** (`Game/Skills/`) — Handles ore smelting
- **CookingService** (`Game/Skills/`) — Handles cooking with burn chance calculation
- **FiremakingService** (`Game/Skills/`) — Handles log burning
- **EquipmentService** (`Game/Items/`) — Equipment requirement validation and equip/unequip
- **LootService** (`Game/Items/`) — Handles looting from pending loot

All services use singleton pattern with injectable dependencies:
```csharp
public static readonly MyService Instance = new(RandomProvider.Instance, DomainEventDispatcher.Instance);
public MyService(IRandomProvider random, IDomainEventDispatcher events) { ... }
```

## Location Implementation

- **Location**: `src/CliScape.Content/Locations/` (subfolders like `Towns/`)
- **Interface**: Implement `ILocation` from `CliScape.Core.World`
- **Static Name**: Define `public static LocationName Name { get; } = new("Location Name");`
- **Required Properties**:
  - `Shop` — Optional shop at location (can be `null`)
  - `Bank` — Optional bank at location (can be `null`)
  - `AvailableNpcs` — Collection of `ICombatableNpc` at this location
  - `FishingSpots` — Collection of `IFishingSpot` at this location (defaults to empty)
  - `Trees` — Collection of `ITree` at this location (defaults to empty)
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
- **Item-on-Item Interactions**: Use `item --name <item> --target <other>` for item-on-item use (e.g., tinderbox on logs)
- **CLI Usage**: `item <name> --examine|--use|--eat|--bury|--drink|--read|--equip`
- **Example**: See `src/CliScape.Content/Items/Food.cs` for edible items

## Skilling System

Players train skills via world interactions and item-on-item mechanics.

### Skill Commands

- **`skills list`** — View all 23 skills with levels, XP, and progress to next level
- **`skills fish [spot]`** — List fishing spots or fish at a specific spot
- **`skills chop [tree]`** — List trees or chop a specific tree type
- **Firemaking**: Use `item --name Tinderbox --target Logs` to burn logs

### World Resources

Resources are defined in `src/CliScape.Content/Resources/`:

- **FishingSpots.cs** — Fishing spot definitions (SmallNetSpot, LureSpot, CageSpot, HarpoonSpot)
- **Trees.cs** — Tree definitions (NormalTree, OakTree, WillowTree, etc.)

### Resource Interfaces

Located in `src/CliScape.Core/World/Resources/`:

- **IFishingSpot** — Fishing spots with spot type, required level, tool, and possible catches
- **ITree** — Trees with type, required level, experience, and log output

### Adding New Skilling Resources

1. Add item IDs to `ItemIds.cs` (logs use 1200-1299, raw fish use 1300-1399)
2. Create item definitions in appropriate class (Logs.cs, RawFish.cs)
3. Register in `ItemRegistry.cs`
4. Add resource definition in `Resources/` folder
5. Add resources to location `FishingSpots` or `Trees` property

### Skill Helpers

- **FiremakingHelper** (`CliScape.Core.Skills`) — Handles tinderbox + logs interactions
- **CookingHelper** (`CliScape.Core.Skills`) — Recipe lookup and burn chance calculations
- **SmithingHelper** (`CliScape.Core.Skills`) — Smelting and smithing recipe definitions
- Use `Player.AddExperience(skill, xp)` to grant experience

### Skill Data Types

Located in `CliScape.Core.Skills/`:
- **SmeltingRecipe** — Record for smelting ore into bars
- **SmithingRecipe** — Record for smithing bars into equipment
- **CookingRecipe** — Record for cooking raw food

## Domain Events

Events are dispatched for cross-cutting concerns via `IDomainEventDispatcher`.

### Event Types (in `CliScape.Core.Events/`)

- **LevelUpEvent** — Skill level increased
- **ExperienceGainedEvent** — XP awarded to a skill
- **CombatStartedEvent** / **CombatEndedEvent** — Combat lifecycle
- **ItemPickedUpEvent** / **ItemDroppedEvent** — Inventory changes
- **SlayerTaskCompletedEvent** — Slayer task finished
- **PlayerDiedEvent** — Player died

### Usage Pattern

```csharp
// Raise events
_eventDispatcher.Raise(new LevelUpEvent("Fishing", 50));

// Subscribe to events
dispatcher.Subscribe<LevelUpEvent>(e => Console.WriteLine($"Level up: {e.SkillName}"));
```

## Randomness & Testability

All random-dependent code uses `IRandomProvider` for testability.

- **Interface**: `IRandomProvider` in `CliScape.Core`
- **Default**: `RandomProvider.Instance` uses `Random.Shared`
- **Testing**: Inject mock implementations for deterministic tests

```csharp
public class MyService
{
    private readonly IRandomProvider _random;
    
    public static readonly MyService Instance = new(RandomProvider.Instance);
    
    public MyService(IRandomProvider random) => _random = random;
    
    public int RollDamage(int maxHit) => _random.Next(0, maxHit + 1);
}
```

## Value Objects

Domain primitives in `CliScape.Core/` for type safety:

- **Gold** — Non-negative coin amounts with display formatting
- **Damage** — Combat damage with hit/miss semantics
- **Hitpoints** — Health points with damage/heal operations
- **Experience** — XP values capped at 200M
- **ItemId** — Item type identifier
- **ItemName** — Item display name
- **NpcName** — NPC display name
- **LocationName** — Location identifier
- **SkillName** — Skill identifier
- **ShopName** — Shop display name

Value objects provide:
- Validation (non-negative, max values)
- Operator overloading (+, -, comparisons)
- Display formatting (1.2M, 50K, etc.)
- Implicit conversion to/from primitives

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

## Clue Scroll System

Clue scrolls are treasure trail items obtained as NPC drops. Players interact with them entirely through the `item` command — no separate command branch.

### Architecture
- **Core models**: `src/CliScape.Core/ClueScrolls/` — `ClueScrollTier` enum (Easy/Medium/Hard/Elite), `ClueStepType` enum (Dig/Search/Talk), `ClueStep` record, `ClueScroll` record, `ClueReward` record, `IClueStepPool` and `IClueRewardTable` interfaces
- **Content definitions**: `src/CliScape.Content/ClueScrolls/` — `ClueStepPool` (step definitions per tier referencing locations), `ClueRewardTable` (reward tables per tier)
- **Items**: `src/CliScape.Content/Items/ClueScrollItems.cs` — Clue scroll and reward casket items (IDs 900–913)
- **Service**: `src/CliScape.Game/ClueScrolls/ClueScrollService.cs` — Assembles clues, validates steps, rolls rewards
- **Wiring**: `src/CliScape.Game/ClueScrolls/ClueScrollWiring.cs` — Connects item actions to service via `ClueScrollActions` static delegates
- **Events**: `ClueStepCompletedEvent`, `ClueScrollCompletedEvent`
- **Persistence**: `ClueScrollSnapshot` / `ClueStepSnapshot` in `PlayerSnapshot`
- **Player state**: `Player.ActiveClue` (nullable `ClueScroll` record)

### Player Interaction Flow
1. Kill an NPC → clue scroll drops → loot it into inventory
2. `item --name "Clue scroll (easy)" --use` → starts trail, shows first hint
3. `item --name "Clue scroll (easy)" --read` → re-read current step hint
4. `walk <location>` → travel to the hinted location
5. `item --name "Clue scroll (easy)" --use` → validates location, advances step
6. Repeat steps 3–5 until all steps are completed
7. On final step completion → scroll consumed, reward casket added to inventory
8. `item --name "Reward casket (easy)" --use` → opens casket, grants random rewards

### Cross-Layer Wiring Pattern
Content-layer items cannot reference Game-layer services directly. The `ClueScrollActions` static class in Core holds `Func` delegates that Game wires at startup via `ClueScrollWiring.Initialize()`. This keeps the dependency chain clean: `Content → Core` (no circular deps).

### Adding New Clue Steps
1. Add `ClueStep` entries to the appropriate tier array in `ClueStepPool.cs`
2. Reference existing `LocationName` values — steps must point to locations that exist
3. Each step needs: `StepType`, `HintText`, `RequiredLocation`, `CompletionText`

### Adding New Clue Rewards
1. Add `ClueReward` entries to the appropriate tier array in `ClueRewardTable.cs`
2. Reference existing `ItemId` values from `ItemIds.cs`
3. Set `Weight` for rarity (higher = more common), and `MinQuantity`/`MaxQuantity`

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

### Test Projects

| Project | Tests | References |
|---------|-------|------------|
| `CliScape.Core.Tests` | Core domain logic: combat calculator, combat sessions, inventory, equipment, shops, skill helpers (cooking, thieving), pending loot | Core, Tests.Shared |
| `CliScape.Game.Tests` | Game services: mining, cooking, smelting, firemaking, woodcutting, equipment, loot, slayer, clue scrolls | Core, Game, Tests.Shared |
| `CliScape.Content.Tests` | Integration/content tests: persistence round-trips, combat engine, domain events, drop tables, fishing, player state | Core, Content, Game, Infrastructure, Tests.Shared |

### Shared Test Helpers (`CliScape.Tests.Shared`)

Non-test library referenced by all test projects:

- **`TestFactory`** — Factory methods for test objects:
  - `CreatePlayer()` — Player with default stats
  - `CreatePlayerWithSkill(skill, level)` — Player with a specific skill level
  - `CreateStubItem(id, ...)` — NSubstitute-based `IItem` stub
  - `CreateStubEquippable(id, name, slot, ...)` — NSubstitute-based `IEquippable` stub
  - `StubLocation` — Minimal `ILocation` implementation
- **`StubEventDispatcher`** — Records all raised events for assertion; provides `AssertRaised<T>()` and `AssertNotRaised<T>()` helpers
- **`StubRandomProvider`** — Deterministic `IRandomProvider`; queue specific return values with `Returns()` / `EnqueueRange()` or set a `WithDefault()` fallback

### Conventions

- **Framework**: xUnit with `[Fact]` and `[Theory]`/`[InlineData]` attributes
- **Naming**: Test classes use `Test*` prefix (e.g., `TestCombatCalculator`, `TestMiningService`)
- **Mocking**: NSubstitute for interface stubs; shared stubs for common test doubles
- **Coverage**: `coverlet.collector` available for coverage reports
- **Pattern**: Tests instantiate services with `StubRandomProvider` and `StubEventDispatcher` for deterministic, isolated behavior

### Running Tests

```bash
dotnet test CliScape.slnx                 # All tests
dotnet test tests/CliScape.Core.Tests     # Core domain tests
dotnet test tests/CliScape.Game.Tests     # Game service tests
dotnet test tests/CliScape.Content.Tests  # Content/integration tests
```

## Git Conventions

- **Commits**: Short imperative messages (e.g., "Add goblin NPC", "Fix combat calculation")
- **PRs**: Describe change, list test commands run, note gameplay impacts
- **Screenshots**: Include CLI output for UI/UX changes
