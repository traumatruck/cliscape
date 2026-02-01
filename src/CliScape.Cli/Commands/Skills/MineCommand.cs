using System.ComponentModel;
using CliScape.Content.Items;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World.Resources;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Mine rocks in the current location for ore.
/// </summary>
[Description("Mine rocks in your current location")]
public class MineCommand : Command<MineCommandSettings>, ICommand
{
    /// <summary>
    ///     Pickaxe item IDs in order of tier.
    /// </summary>
    private static readonly (ItemId Id, PickaxeTier Tier, int RequiredLevel)[] Pickaxes =
    [
        (new ItemId(701), PickaxeTier.Bronze, 1), // Bronze pickaxe
        (new ItemId(710), PickaxeTier.Iron, 1), // Iron pickaxe
        (new ItemId(711), PickaxeTier.Steel, 6), // Steel pickaxe
        (new ItemId(712), PickaxeTier.Mithril, 21), // Mithril pickaxe
        (new ItemId(713), PickaxeTier.Adamant, 31), // Adamant pickaxe
        (new ItemId(714), PickaxeTier.Rune, 41) // Rune pickaxe
    ];

    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "mine";

    public override int Execute(CommandContext context, MineCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // If no rock specified, list available rocks
        if (string.IsNullOrWhiteSpace(settings.RockType))
        {
            return ListRocks(location.MiningRocks, player);
        }

        // Find the matching rock
        var rock = FindRock(location.MiningRocks, settings.RockType);
        if (rock is null)
        {
            AnsiConsole.MarkupLine($"[red]There is no '{settings.RockType}' rock here.[/]");
            AnsiConsole.MarkupLine("[dim]Use 'skills mine' without arguments to see available rocks.[/]");
            return (int)ExitCode.Failure;
        }

        var requestedCount = settings.Count ?? 1;
        var maxCount = Math.Min(requestedCount, rock.MaxActions);

        if (requestedCount > rock.MaxActions)
        {
            AnsiConsole.MarkupLine(
                $"[dim]Note: {rock.Name} can only be mined {rock.MaxActions} time(s) before it depletes.[/]");
        }

        return MineRockMultiple(player, rock, maxCount);
    }

    private static int ListRocks(IReadOnlyList<IMiningRock> rocks, Player player)
    {
        if (rocks.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no rocks here to mine.[/]");
            return (int)ExitCode.Success;
        }

        var miningLevel = player.GetSkillLevel(SkillConstants.MiningSkillName).Value;

        AnsiConsole.MarkupLine("[bold]Rocks at this location:[/]\n");

        var table = new Table()
            .AddColumn("Rock")
            .AddColumn("Req. Level")
            .AddColumn("Experience")
            .AddColumn("Required Pickaxe")
            .AddColumn("Ore");

        foreach (var rock in rocks)
        {
            var levelColor = miningLevel >= rock.RequiredLevel ? "green" : "red";
            var oreName = GetOreName(rock.OreItemId);

            table.AddRow(
                rock.Name,
                $"[{levelColor}]{rock.RequiredLevel}[/]",
                rock.Experience.ToString(),
                rock.RequiredPickaxe.ToString(),
                oreName
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[dim]Use 'skills mine <rock>' to mine a rock (e.g., 'skills mine copper').[/]");
        AnsiConsole.MarkupLine("[dim]Use 'skills mine <rock> -c <count>' to mine multiple times.[/]");

        return (int)ExitCode.Success;
    }

    private int MineRockMultiple(Player player, IMiningRock rock, int count)
    {
        var miningSkill = player.GetSkill(SkillConstants.MiningSkillName);
        var miningLevel = miningSkill.Level.Value;

        // Check level requirement
        if (miningLevel < rock.RequiredLevel)
        {
            AnsiConsole.MarkupLine($"[red]You need level {rock.RequiredLevel} Mining to mine this rock.[/]");
            return (int)ExitCode.Failure;
        }

        // Check for appropriate pickaxe
        if (!HasAppropriatePickaxe(player, rock.RequiredPickaxe, miningLevel, out var pickaxeName))
        {
            AnsiConsole.MarkupLine(
                $"[red]You need at least a {rock.RequiredPickaxe} pickaxe to mine this rock.[/]");
            return (int)ExitCode.Failure;
        }

        // Get the ore item
        var oreItem = ItemRegistry.GetById(rock.OreItemId);
        if (oreItem is null)
        {
            AnsiConsole.MarkupLine("[red]Something went wrong while mining.[/]");
            return (int)ExitCode.Failure;
        }

        var oresObtained = 0;
        var totalXp = 0;

        for (var i = 0; i < count; i++)
        {
            // Check inventory space
            if (player.Inventory.IsFull)
            {
                if (oresObtained == 0)
                {
                    AnsiConsole.MarkupLine("[red]Your inventory is full.[/]");
                    return (int)ExitCode.Failure;
                }

                AnsiConsole.MarkupLine("[yellow]Your inventory is full.[/]");
                break;
            }

            // Add to inventory
            if (!player.Inventory.TryAdd(oreItem))
            {
                break;
            }

            // Grant experience
            Player.AddExperience(miningSkill, rock.Experience);
            oresObtained++;
            totalXp += rock.Experience;
        }

        if (oresObtained == 1)
        {
            AnsiConsole.MarkupLine($"[green]You swing your {pickaxeName} at the rock and get some {oreItem.Name}.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                $"[green]You swing your {pickaxeName} at the rock and get {oresObtained}x {oreItem.Name}.[/]");
        }

        AnsiConsole.MarkupLine($"[dim]You gain {totalXp:N0} Mining experience.[/]");

        _gameState.Save();
        return (int)ExitCode.Success;
    }

    private static IMiningRock? FindRock(IReadOnlyList<IMiningRock> rocks, string rockType)
    {
        return rocks.FirstOrDefault(r =>
            r.RockType.ToString().Equals(rockType, StringComparison.OrdinalIgnoreCase) ||
            r.Name.Contains(rockType, StringComparison.OrdinalIgnoreCase));
    }

    private static bool HasAppropriatePickaxe(Player player, PickaxeTier requiredTier, int miningLevel,
        out string pickaxeName)
    {
        pickaxeName = string.Empty;

        // Check from best to worst pickaxe
        foreach (var (id, tier, requiredLevel) in Pickaxes.Reverse())
        {
            // Skip if player doesn't have the level for this pickaxe
            if (miningLevel < requiredLevel)
            {
                continue;
            }

            // Check inventory for this pickaxe
            var inInventory = player.Inventory.GetItems()
                .FirstOrDefault(slot => slot.Item.Id == id);

            if (inInventory != default)
            {
                // Check if this pickaxe is good enough for the rock
                if ((int)tier >= (int)requiredTier)
                {
                    pickaxeName = inInventory.Item.Name.Value;
                    return true;
                }
            }

            // Check equipped weapon slot for pickaxe
            var weapon = player.Equipment.GetEquipped(EquipmentSlot.Weapon);
            if (weapon?.Id == id)
            {
                if ((int)tier >= (int)requiredTier)
                {
                    pickaxeName = weapon.Name.Value;
                    return true;
                }
            }
        }

        return false;
    }

    private static string GetOreName(ItemId id)
    {
        var item = ItemRegistry.GetById(id);
        return item?.Name.Value ?? "Unknown ore";
    }
}