# CliScape

An OSRS-inspired CLI RPG built with .NET 10.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Building & Running

**Prerequisites:** .NET 10 SDK

```bash
# Build
dotnet build CliScape.slnx

# Run
dotnet run --project src/CliScape.Cli

# Or use the convenience script
./cliscape.sh

# Run tests
dotnet test
```

Your save file is stored at `~/.local/share/CliScape/save.bin`.

## Commands

| Command | Description |
|---------|-------------|
| `status` | View character status |
| `skills` | Train and view skills |
| `walk` | Travel to a new location |
| `combat` | Fight NPCs and loot drops |
| `inventory` | Manage your 28-slot inventory |
| `item` | Interact with items |
| `shop` | Browse and trade with shops |
| `equipment` | View and manage gear |
| `slayer` | Get and track slayer tasks |
| `diary` | Achievement diaries and rewards |
| `bank` | Deposit and withdraw items |

Most commands have subcommands — run any command without arguments to see usage.

## Game Mechanics

### Skills

23 skills across four categories:

- **Combat** — Attack, Strength, Defence, Hitpoints, Ranged, Prayer, Magic
- **Gathering** — Mining, Fishing, Woodcutting, Farming, Hunter
- **Artisan** — Smithing, Crafting, Fletching, Cooking, Firemaking, Herblore, Construction
- **Support** — Agility, Thieving, Slayer, Runecraft

Levels range from 1 to 99 with a 200M XP cap, using the OSRS-accurate level formula.

### Combat

Turn-based combat with four styles that determine XP distribution:

- **Accurate** — Attack XP
- **Aggressive** — Strength XP
- **Defensive** — Defence XP
- **Controlled** — Shared Attack/Strength/Defence XP

You can flee from encounters. Defeated NPCs drop loot that can be collected.

### Skilling

Trainable gathering and production skills:

- **Fishing** (`skills fish`) — Catch fish at fishing spots
- **Woodcutting** (`skills chop`) — Chop trees for logs
- **Mining** (`skills mine`) — Mine rocks for ore
- **Smelting** (`skills smelt`) — Smelt ore into bars
- **Smithing** (`skills smith`) — Smith bars into items
- **Cooking** (`skills cook`) — Cook raw food
- **Thieving** (`skills thieve`) — Pickpocket NPCs

### Slayer

Accept tasks from slayer masters to kill a specific number of a target creature. Completing tasks grants Slayer XP.

### Clue Scrolls

Multi-step location-based puzzles in four tiers:

| Tier | Steps | Rewards |
|------|-------|---------|
| Easy | 2–3 | Basic |
| Medium | 3–4 | Moderate |
| Hard | 4–5 | Valuable |
| Elite | 5–6 | Best |

### Achievement Diaries

Per-location diaries (Lumbridge, Falador, Varrock) with Easy, Medium, Hard, and Elite tiers. Complete tasks to unlock claimable rewards.

### Items & Equipment

- **Inventory** — 28 slots for carrying items
- **Equipment** — 11 slots: Head, Cape, Neck, Ammo, Weapon, Body, Shield, Legs, Hands, Feet, Ring
- **Shops** — Buy and sell items from NPC shops
- **Bank** — Long-term item storage

## Project Structure

| Project | Description |
|---------|-------------|
| `CliScape.Cli` | Command-line interface and entry point |
| `CliScape.Core` | Domain models, enums, and core abstractions |
| `CliScape.Game` | Game services and business logic |
| `CliScape.Content` | Game data definitions (items, NPCs, locations) |
| `CliScape.Infrastructure` | Persistence, configuration, and DI wiring |
