using CliScape.Core.Items;
using CliScape.Core.World.Resources;

namespace CliScape.Core.Skills;

/// <summary>
///     Handles Thieving skill operations.
/// </summary>
public static class ThievingHelper
{
    private static readonly Random Random = new();

    /// <summary>
    ///     Calculates the success chance for a thieving attempt.
    /// </summary>
    /// <param name="target">The thieving target.</param>
    /// <param name="thievingLevel">The player's thieving level.</param>
    /// <returns>The chance of success as a value between 0.0 and 1.0.</returns>
    public static double CalculateSuccessChance(IThievingTarget target, int thievingLevel)
    {
        // Base chance from the target
        var baseChance = target.BaseSuccessChance;

        // Level bonus: each level above requirement adds 1% success chance
        var levelBonus = (thievingLevel - target.RequiredLevel) * 0.01;

        // Cap at 95% success rate
        return Math.Min(0.95, Math.Max(0.0, baseChance + levelBonus));
    }

    /// <summary>
    ///     Determines if a thieving attempt succeeds.
    /// </summary>
    public static bool IsSuccessful(double successChance)
    {
        return Random.NextDouble() < successChance;
    }

    /// <summary>
    ///     Selects a random loot item from the target's loot table.
    /// </summary>
    /// <param name="target">The thieving target.</param>
    /// <returns>The selected loot with quantity.</returns>
    public static (ItemId itemId, int quantity) SelectLoot(IThievingTarget target)
    {
        if (target.PossibleLoot.Count == 0)
        {
            return (new ItemId(1), 1); // Default to coins
        }

        // Calculate total weight
        var totalWeight = 0;
        foreach (var loot in target.PossibleLoot)
        {
            totalWeight += loot.Weight;
        }

        // Select random loot based on weight
        var roll = Random.Next(totalWeight);
        var currentWeight = 0;

        foreach (var loot in target.PossibleLoot)
        {
            currentWeight += loot.Weight;
            if (roll < currentWeight)
            {
                var quantity = Random.Next(loot.MinQuantity, loot.MaxQuantity + 1);
                return (loot.ItemId, quantity);
            }
        }

        // Fallback to first item
        var firstLoot = target.PossibleLoot[0];
        return (firstLoot.ItemId, firstLoot.MinQuantity);
    }

    /// <summary>
    ///     Gets the failure message for a thieving attempt.
    /// </summary>
    public static string GetFailureMessage(IThievingTarget target)
    {
        return target.TargetType switch
        {
            ThievingTargetType.Stall => "You fail to steal anything. A guard notices you!",
            ThievingTargetType.Npc => $"You fail to pick the {target.Name}'s pocket. They notice!",
            _ => "You fail to steal anything."
        };
    }

    /// <summary>
    ///     Gets the success message for a thieving attempt.
    /// </summary>
    public static string GetSuccessMessage(IThievingTarget target, string itemName, int quantity)
    {
        var quantityText = quantity > 1 ? $"{quantity}x " : "";

        return target.TargetType switch
        {
            ThievingTargetType.Stall => $"You successfully steal {quantityText}{itemName} from the {target.Name}.",
            ThievingTargetType.Npc =>
                $"You successfully pick the {target.Name}'s pocket and find {quantityText}{itemName}.",
            _ => $"You successfully steal {quantityText}{itemName}."
        };
    }
}