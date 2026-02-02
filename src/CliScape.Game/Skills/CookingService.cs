using CliScape.Core;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Default implementation of <see cref="ICookingService" />.
/// </summary>
public sealed class CookingService : ICookingService
{
    public static readonly CookingService Instance = new(
        RandomProvider.Instance,
        DomainEventDispatcher.Instance);

    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IRandomProvider _random;

    public CookingService(IRandomProvider random, IDomainEventDispatcher eventDispatcher)
    {
        _random = random;
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public (bool CanCook, string? ErrorMessage) CanCook(Player player, int requiredLevel)
    {
        var cookingLevel = player.GetSkillLevel(SkillConstants.CookingSkillName).Value;

        if (cookingLevel < requiredLevel)
        {
            return (false, $"You need level {requiredLevel} Cooking to cook this.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public CookingResult Cook(
        Player player,
        ICookingRange range,
        CookingRecipe recipe,
        int count,
        Func<ItemId, IItem?> itemResolver)
    {
        var cookingSkill = player.GetSkill(SkillConstants.CookingSkillName);
        var cookingLevel = cookingSkill.Level.Value;

        var rawItem = itemResolver(recipe.RawItemId);
        var cookedItem = itemResolver(recipe.CookedItemId);
        var burntItem = itemResolver(recipe.BurntItemId);

        if (rawItem is null || cookedItem is null || burntItem is null)
        {
            return new CookingResult(false, "Something went wrong while cooking.", 0, 0, null, 0);
        }

        var itemsCooked = 0;
        var itemsBurnt = 0;
        var totalXp = 0;
        var levelBefore = cookingSkill.Level.Value;

        // Calculate burn chance
        var burnChance = CalculateBurnChance(recipe, cookingLevel, range.BurnChanceReduction);

        for (var i = 0; i < count; i++)
        {
            // Check for raw food
            if (!HasItem(player, recipe.RawItemId))
            {
                if (itemsCooked == 0 && itemsBurnt == 0)
                {
                    return new CookingResult(false, $"You don't have any {rawItem.Name}.", 0, 0, null, 0);
                }

                break;
            }

            // Remove raw food
            RemoveItem(player, recipe.RawItemId, 1);

            // Check if it burns
            if (_random.NextDouble() < burnChance)
            {
                player.Inventory.TryAdd(burntItem);
                itemsBurnt++;
            }
            else
            {
                player.Inventory.TryAdd(cookedItem);
                Player.AddExperience(cookingSkill, recipe.Experience);
                itemsCooked++;
                totalXp += recipe.Experience;
            }
        }

        if (itemsCooked == 0 && itemsBurnt == 0)
        {
            return new CookingResult(false, "You didn't cook anything.", 0, 0, null, 0);
        }

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = cookingSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.CookingSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.CookingSkillName, levelAfter));
        }

        if (totalXp > 0)
        {
            _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.CookingSkillName, totalXp,
                cookingSkill.Level.Experience));
        }

        var message = BuildCookingMessage(itemsCooked, itemsBurnt, rawItem.Name.Value, cookedItem.Name.Value);

        return new CookingResult(true, message, itemsCooked, itemsBurnt, cookedItem.Name.Value, totalXp, levelUp);
    }

    private static double CalculateBurnChance(CookingRecipe recipe, int cookingLevel, int burnChanceReduction)
    {
        if (cookingLevel >= recipe.StopBurnLevel)
        {
            return 0.0;
        }

        var levelRange = recipe.StopBurnLevel - recipe.RequiredLevel;
        var playerProgress = cookingLevel - recipe.RequiredLevel;

        if (levelRange <= 0 || playerProgress < 0)
        {
            return 0.6;
        }

        var baseBurnChance = 0.6 * (1.0 - (double)playerProgress / levelRange);
        var reduction = burnChanceReduction / 100.0;
        var finalChance = baseBurnChance * (1.0 - reduction);

        return Math.Max(0.0, Math.Min(1.0, finalChance));
    }

    private static string BuildCookingMessage(int cooked, int burnt, string rawName, string cookedName)
    {
        var parts = new List<string>();

        if (cooked > 0)
        {
            parts.Add(cooked == 1
                ? $"Successfully cooked {rawName}"
                : $"Successfully cooked {cooked}x {cookedName}");
        }

        if (burnt > 0)
        {
            parts.Add(burnt == 1
                ? $"Burnt 1x {rawName}"
                : $"Burnt {burnt}x {rawName}");
        }

        return string.Join(". ", parts) + ".";
    }

    private static bool HasItem(Player player, ItemId itemId)
    {
        return player.Inventory.GetItems().Any(slot => slot.Item.Id == itemId);
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
