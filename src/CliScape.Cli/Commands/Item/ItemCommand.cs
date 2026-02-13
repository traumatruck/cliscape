using System.ComponentModel;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Skills;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Item;

/// <summary>
///     Interact with an item in inventory using a specific action (eat, bury, use, examine, etc.).
/// </summary>
[Description("Interact with an item in your inventory")]
public class ItemCommand(GameState gameState) : Command<ItemCommandSettings>, ICommand
{
    public static string CommandName => "item";

    public override int Execute(CommandContext context, ItemCommandSettings settings,
        CancellationToken cancellationToken)
    {
        // Validate that exactly one of name or slot is provided
        if (settings.ItemName is null && settings.ItemSlot is null)
        {
            AnsiConsole.MarkupLine("[red]You must specify either --name or --slot to identify the item.[/]");
            return (int)ExitCode.Failure;
        }

        if (settings.ItemName is not null && settings.ItemSlot is not null)
        {
            AnsiConsole.MarkupLine("[red]You cannot specify both --name and --slot. Choose one.[/]");
            return (int)ExitCode.Failure;
        }

        var player = gameState.GetPlayer();
        var inventory = player.Inventory;

        // Find the item by name or index
        IItem? item = null;

        if (settings.ItemName is not null)
        {
            item = FindItemByName(inventory, settings.ItemName);
        }
        else if (settings.ItemSlot is not null)
        {
            item = FindItemBySlot(inventory, settings.ItemSlot.Value);
        }

        if (item is null)
        {
            var identifier = settings.ItemName ?? $"slot #{settings.ItemSlot}";
            AnsiConsole.MarkupLine($"[red]You don't have any '{identifier}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        // Handle item-on-item interaction when --target is specified
        if (settings.TargetItemName is not null)
        {
            return ExecuteItemOnItem(item, settings.TargetItemName, player, inventory);
        }

        // Handle examine action
        if (settings.Examine)
        {
            return ExecuteExamine(item);
        }

        // Determine which action was requested
        var actionType = DetermineAction(settings);

        if (actionType is null)
        {
            return ShowAvailableActions(item);
        }

        return ExecuteAction(item, actionType.Value, player, inventory);
    }

    private int ExecuteItemOnItem(IItem sourceItem, string targetItemName, Player player,
        Core.Items.Inventory inventory)
    {
        // Find the target item
        var targetItem = FindItemByName(inventory, targetItemName);
        if (targetItem is null)
        {
            AnsiConsole.MarkupLine($"[red]You don't have any '{targetItemName}' in your inventory.[/]");
            return (int)ExitCode.Failure;
        }

        // Check for firemaking (tinderbox + logs)
        if (FiremakingHelper.CanUseForFiremaking(sourceItem, targetItem, out _, out var logs) && logs is not null)
        {
            if (FiremakingHelper.TryLightLogs(player, logs, out var message))
            {
                AnsiConsole.MarkupLine($"[green]{message}[/]");
                gameState.Save();
                return (int)ExitCode.Success;
            }

            AnsiConsole.MarkupLine($"[red]{message}[/]");
            return (int)ExitCode.Failure;
        }

        // No valid interaction found
        AnsiConsole.MarkupLine(
            $"[yellow]Nothing interesting happens when you use {sourceItem.Name} on {targetItem.Name}.[/]");
        return (int)ExitCode.Success;
    }

    private static IItem? FindItemByName(Core.Items.Inventory inventory, string name)
    {
        var itemEntry = inventory.GetItems()
            .FirstOrDefault(i => i.Item.Name.Value.Equals(name, StringComparison.OrdinalIgnoreCase));

        return itemEntry == default ? null : itemEntry.Item;
    }

    private static IItem? FindItemBySlot(Core.Items.Inventory inventory, int slotNumber)
    {
        // Convert from 1-based user slot number to 0-based internal index
        var slotIndex = slotNumber - 1;

        if (slotIndex < 0 || slotIndex >= Core.Items.Inventory.MaxSlots)
        {
            return null;
        }

        // Get the item directly from the specified slot
        var slot = inventory.GetSlot(slotIndex);

        return slot.IsEmpty ? null : slot.Item;
    }

    private static ItemAction? DetermineAction(ItemCommandSettings settings)
    {
        if (settings.Eat)
        {
            return ItemAction.Eat;
        }

        if (settings.Bury)
        {
            return ItemAction.Bury;
        }

        if (settings.Use)
        {
            return ItemAction.Use;
        }

        if (settings.Drink)
        {
            return ItemAction.Drink;
        }

        if (settings.Read)
        {
            return ItemAction.Read;
        }

        if (settings.Equip)
        {
            return ItemAction.Equip;
        }

        return null;
    }

    private static int ShowAvailableActions(IItem item)
    {
        if (item is not IActionableItem actionableItem || actionableItem.Actions.Count == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]{item.Name} has no special actions available.[/]");
            AnsiConsole.MarkupLine("[dim]You can examine or drop it from your inventory.[/]");
            return (int)ExitCode.Success;
        }

        AnsiConsole.MarkupLine($"[yellow]{item.Name}[/] can be used with the following actions:");
        foreach (var action in actionableItem.Actions)
        {
            var actionFlag = GetActionFlag(action.ActionType);
            AnsiConsole.MarkupLine($"  [green]--{actionFlag}[/] - {action.Description}");
        }

        return (int)ExitCode.Success;
    }

    private int ExecuteAction(IItem item, ItemAction actionType, Player player, Core.Items.Inventory inventory)
    {
        // Special handling for equip (not part of the IItemAction system)
        if (actionType is ItemAction.Equip)
        {
            return HandleEquip(item, player, inventory);
        }

        // Get the action from the item
        if (item is not IActionableItem actionableItem)
        {
            AnsiConsole.MarkupLine($"[red]You can't {actionType.ToString().ToLower()} a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        var action = actionableItem.GetAction(actionType);
        if (action is null)
        {
            AnsiConsole.MarkupLine($"[red]You can't {actionType.ToString().ToLower()} a {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Remove the item if the action consumes it
        if (action.ConsumesItem)
        {
            var removed = inventory.Remove(item);
            if (removed == 0)
            {
                AnsiConsole.MarkupLine("[red]Failed to use the item.[/]");
                return (int)ExitCode.Failure;
            }
        }

        // Execute the action
        var message = action.Execute(item, player);
        AnsiConsole.MarkupLine($"[green]{message}[/]");

        // Show health for eat actions
        if (actionType == ItemAction.Eat)
        {
            AnsiConsole.MarkupLine($"[dim]Health: {player.CurrentHealth}/{player.MaximumHealth}[/]");
        }

        return (int)ExitCode.Success;
    }

    private int HandleEquip(IItem item, Player player, Core.Items.Inventory inventory)
    {
        if (item is not IEquippable equippable)
        {
            AnsiConsole.MarkupLine($"[red]You cannot equip {item.Name}.[/]");
            return (int)ExitCode.Failure;
        }

        // Check requirements
        if (!MeetsRequirements(player, equippable, out var failMessage))
        {
            AnsiConsole.MarkupLine($"[red]{failMessage}[/]");
            return (int)ExitCode.Failure;
        }

        // Remove from inventory
        inventory.Remove(item);

        // Equip the item, getting any previously equipped item back
        var previous = player.Equipment.Equip(equippable);

        // If there was a previous item, put it in inventory
        if (previous is not null)
        {
            AnsiConsole.MarkupLine(!inventory.TryAdd(previous)
                ? $"[yellow]Warning: Could not return {previous.Name} to inventory.[/]"
                : $"You unequip your [yellow]{previous.Name}[/].");
        }

        AnsiConsole.MarkupLine($"You equip the [yellow]{equippable.Name}[/].");
        return (int)ExitCode.Success;
    }

    private static bool MeetsRequirements(Player player, IEquippable equippable, out string message)
    {
        var skills = player.Skills;

        var attack = skills.FirstOrDefault(s => s.Name.Name == "Attack")?.Level.Value ?? 1;
        var strength = skills.FirstOrDefault(s => s.Name.Name == "Strength")?.Level.Value ?? 1;
        var defence = skills.FirstOrDefault(s => s.Name.Name == "Defence")?.Level.Value ?? 1;
        var ranged = skills.FirstOrDefault(s => s.Name.Name == "Ranged")?.Level.Value ?? 1;
        var magic = skills.FirstOrDefault(s => s.Name.Name == "Magic")?.Level.Value ?? 1;

        if (equippable.RequiredAttackLevel > attack)
        {
            message = $"You need {equippable.RequiredAttackLevel} Attack to equip this.";
            return false;
        }

        if (equippable.RequiredStrengthLevel > strength)
        {
            message = $"You need {equippable.RequiredStrengthLevel} Strength to equip this.";
            return false;
        }

        if (equippable.RequiredDefenceLevel > defence)
        {
            message = $"You need {equippable.RequiredDefenceLevel} Defence to equip this.";
            return false;
        }

        if (equippable.RequiredRangedLevel > ranged)
        {
            message = $"You need {equippable.RequiredRangedLevel} Ranged to equip this.";
            return false;
        }

        if (equippable.RequiredMagicLevel > magic)
        {
            message = $"You need {equippable.RequiredMagicLevel} Magic to equip this.";
            return false;
        }

        message = string.Empty;
        return true;
    }

    private static string GetActionFlag(ItemAction action)
    {
        return action switch
        {
            ItemAction.Examine => "examine",
            ItemAction.Use => "use",
            ItemAction.Eat => "eat",
            ItemAction.Bury => "bury",
            ItemAction.Drink => "drink",
            ItemAction.Read => "read",
            ItemAction.Equip => "equip",
            _ => action.ToString().ToLower()
        };
    }

    #region Examine functionality

    private static int ExecuteExamine(IItem item)
    {
        // Show examine text (classic OSRS style)
        AnsiConsole.MarkupLine($"[italic]{item.ExamineText}[/]");
        AnsiConsole.WriteLine();

        // Show basic info
        var infoTable = new Table().HideHeaders().NoBorder();
        infoTable.AddColumn("Property");
        infoTable.AddColumn("Value");

        infoTable.AddRow("Value", $"[yellow]{item.BaseValue:N0} gp[/]");
        infoTable.AddRow("Tradeable", item.IsTradeable ? "[green]Yes[/]" : "[red]No[/]");
        infoTable.AddRow("Stackable", item.IsStackable ? "[green]Yes[/]" : "[dim]No[/]");

        AnsiConsole.Write(infoTable);

        // If equippable, show equipment stats
        if (item is IEquippable equippable)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold]Equipment Stats ({equippable.Slot})[/]");

            ShowEquipmentStats(equippable);
            ShowRequirements(equippable);
        }

        return (int)ExitCode.Success;
    }

    private static void ShowEquipmentStats(IEquippable equippable)
    {
        var stats = equippable.Stats;

        // Attack bonuses
        var attackTable = new Table().Title("[bold]Attack Bonuses[/]").NoBorder();
        attackTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        attackTable.AddRow(
            FormatBonus(stats.StabAttackBonus),
            FormatBonus(stats.SlashAttackBonus),
            FormatBonus(stats.CrushAttackBonus),
            FormatBonus(stats.RangedAttackBonus),
            FormatBonus(stats.MagicAttackBonus)
        );
        AnsiConsole.Write(attackTable);

        // Defence bonuses
        var defenceTable = new Table().Title("[bold]Defence Bonuses[/]").NoBorder();
        defenceTable.AddColumn("Stab").AddColumn("Slash").AddColumn("Crush").AddColumn("Ranged").AddColumn("Magic");
        defenceTable.AddRow(
            FormatBonus(stats.StabDefenceBonus),
            FormatBonus(stats.SlashDefenceBonus),
            FormatBonus(stats.CrushDefenceBonus),
            FormatBonus(stats.RangedDefenceBonus),
            FormatBonus(stats.MagicDefenceBonus)
        );
        AnsiConsole.Write(defenceTable);

        // Other bonuses
        var otherTable = new Table().Title("[bold]Other Bonuses[/]").NoBorder();
        otherTable.AddColumn("Melee Str").AddColumn("Ranged Str").AddColumn("Magic Dmg").AddColumn("Prayer");
        otherTable.AddRow(
            FormatBonus(stats.MeleeStrengthBonus),
            FormatBonus(stats.RangedStrengthBonus),
            $"{stats.MagicDamageBonus}%",
            FormatBonus(stats.PrayerBonus)
        );
        AnsiConsole.Write(otherTable);

        // Attack speed for weapons
        if (equippable.Slot == EquipmentSlot.Weapon)
        {
            AnsiConsole.MarkupLine($"Attack Speed: [cyan]{stats.AttackSpeed}[/] ({stats.AttackSpeed * 0.6:F1}s)");
        }
    }

    private static void ShowRequirements(IEquippable equippable)
    {
        var hasRequirements = equippable.RequiredAttackLevel > 0 ||
                              equippable.RequiredStrengthLevel > 0 ||
                              equippable.RequiredDefenceLevel > 0 ||
                              equippable.RequiredRangedLevel > 0 ||
                              equippable.RequiredMagicLevel > 0;

        if (!hasRequirements)
        {
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Requirements[/]");

        if (equippable.RequiredAttackLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Attack: [cyan]{equippable.RequiredAttackLevel}[/]");
        }

        if (equippable.RequiredStrengthLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Strength: [cyan]{equippable.RequiredStrengthLevel}[/]");
        }

        if (equippable.RequiredDefenceLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Defence: [cyan]{equippable.RequiredDefenceLevel}[/]");
        }

        if (equippable.RequiredRangedLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Ranged: [cyan]{equippable.RequiredRangedLevel}[/]");
        }

        if (equippable.RequiredMagicLevel > 0)
        {
            AnsiConsole.MarkupLine($"  Magic: [cyan]{equippable.RequiredMagicLevel}[/]");
        }
    }

    private static string FormatBonus(int bonus)
    {
        return bonus switch
        {
            > 0 => $"[green]+{bonus}[/]",
            < 0 => $"[red]{bonus}[/]",
            _ => "[dim]0[/]"
        };
    }

    #endregion
}