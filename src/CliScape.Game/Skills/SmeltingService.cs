using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Default implementation of <see cref="ISmeltingService" />.
/// </summary>
public sealed class SmeltingService : ISmeltingService
{
    public static readonly SmeltingService Instance = new(DomainEventDispatcher.Instance);
    private readonly IDomainEventDispatcher _eventDispatcher;

    public SmeltingService(IDomainEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public (bool CanSmelt, string? ErrorMessage) CanSmelt(Player player, int requiredLevel)
    {
        var smithingLevel = player.GetSkillLevel(SkillConstants.SmithingSkillName).Value;

        if (smithingLevel < requiredLevel)
        {
            return (false, $"You need level {requiredLevel} Smithing to smelt this bar.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public SmeltingResult Smelt(
        Player player,
        SmeltingRecipe recipe,
        int count,
        Func<ItemId, IItem?> itemResolver)
    {
        var smithingSkill = player.GetSkill(SkillConstants.SmithingSkillName);

        var barItem = itemResolver(recipe.ResultBarId);
        var primaryOre = itemResolver(recipe.PrimaryOreId);
        var secondaryOre = recipe.SecondaryOreId is not null ? itemResolver(recipe.SecondaryOreId.Value) : null;

        if (barItem is null || primaryOre is null)
        {
            return new SmeltingResult(false, "Something went wrong while smelting.", 0, null, 0);
        }

        var barsSmelted = 0;
        var totalXp = 0;
        var levelBefore = smithingSkill.Level.Value;

        for (var i = 0; i < count; i++)
        {
            if (player.Inventory.IsFull)
            {
                if (barsSmelted == 0)
                {
                    return new SmeltingResult(false, "Your inventory is full.", 0, null, 0);
                }

                break;
            }

            // Check for primary ore
            if (!HasItem(player, recipe.PrimaryOreId))
            {
                if (barsSmelted == 0)
                {
                    return new SmeltingResult(false, $"You don't have any {primaryOre.Name}.", 0, null, 0);
                }

                break;
            }

            // Check for secondary ore if needed
            if (recipe.SecondaryOreId is not null)
            {
                var secondaryCount = CountItem(player, recipe.SecondaryOreId.Value);
                if (secondaryCount < recipe.SecondaryOreCount)
                {
                    if (barsSmelted == 0)
                    {
                        return new SmeltingResult(
                            false,
                            $"You need {recipe.SecondaryOreCount}x {secondaryOre?.Name.Value ?? "secondary ore"} but only have {secondaryCount}.",
                            0, null, 0);
                    }

                    break;
                }
            }

            // Remove ores
            RemoveItem(player, recipe.PrimaryOreId, 1);
            if (recipe.SecondaryOreId is not null)
            {
                RemoveItem(player, recipe.SecondaryOreId.Value, recipe.SecondaryOreCount);
            }

            // Add bar to inventory
            player.Inventory.TryAdd(barItem);

            // Grant experience
            Player.AddExperience(smithingSkill, recipe.Experience);
            barsSmelted++;
            totalXp += recipe.Experience;
        }

        if (barsSmelted == 0)
        {
            return new SmeltingResult(false, "You didn't smelt anything.", 0, null, 0);
        }

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = smithingSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.SmithingSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.SmithingSkillName, levelAfter));
        }

        _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.SmithingSkillName, totalXp,
            smithingSkill.Level.Experience));

        var message = barsSmelted == 1
            ? $"You smelt the ore and create a {barItem.Name}."
            : $"You smelt the ores and create {barsSmelted}x {barItem.Name}.";

        return new SmeltingResult(true, message, barsSmelted, barItem.Name.Value, totalXp, levelUp);
    }

    private static bool HasItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems().Any(slot => slot.Item.Id == itemId);
    }

    private static int CountItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems()
            .Where(slot => slot.Item.Id == itemId)
            .Sum(slot => slot.Quantity);
    }

    private static void RemoveItem(Player player, ItemId itemId, int quantity)
    {
        var remaining = quantity;
        foreach (var slot in player.Inventory.GetItems().Where(s => s.Item.Id == itemId))
        {
            var toRemove = Math.Min(slot.Quantity, remaining);
            player.Inventory.Remove(slot.Item, toRemove);
            remaining -= toRemove;
            if (remaining <= 0)
            {
                break;
            }
        }
    }
}
