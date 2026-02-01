using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Core.Skills;

/// <summary>
///     Handles Firemaking skill interactions between items.
/// </summary>
public static class FiremakingHelper
{
    /// <summary>
    ///     Experience values for burning different log types.
    /// </summary>
    private static readonly Dictionary<ItemId, int> LogExperience = new()
    {
        { ItemIds.Logs, 40 },
        { ItemIds.OakLogs, 60 },
        { ItemIds.WillowLogs, 90 },
        { ItemIds.MapleLogs, 135 },
        { ItemIds.YewLogs, 203 },
        { ItemIds.MagicLogs, 304 }
    };

    /// <summary>
    ///     Minimum Firemaking level required for each log type.
    /// </summary>
    private static readonly Dictionary<ItemId, int> LogLevelRequirements = new()
    {
        { ItemIds.Logs, 1 },
        { ItemIds.OakLogs, 15 },
        { ItemIds.WillowLogs, 30 },
        { ItemIds.MapleLogs, 45 },
        { ItemIds.YewLogs, 60 },
        { ItemIds.MagicLogs, 75 }
    };

    /// <summary>
    ///     Checks if an item is a tinderbox.
    /// </summary>
    public static bool IsTinderbox(IItem item)
    {
        return item.Id == ItemIds.Tinderbox;
    }

    /// <summary>
    ///     Checks if an item is a type of log that can be burned.
    /// </summary>
    public static bool IsLog(IItem item)
    {
        return LogExperience.ContainsKey(item.Id);
    }

    /// <summary>
    ///     Attempts to light logs using a tinderbox.
    /// </summary>
    /// <param name="player">The player attempting to light the fire.</param>
    /// <param name="logs">The logs to burn.</param>
    /// <param name="message">The result message.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public static bool TryLightLogs(Player player, IItem logs, out string message)
    {
        if (!IsLog(logs))
        {
            message = $"You can't light a {logs.Name}.";
            return false;
        }

        var firemakingSkill = player.GetSkill(SkillConstants.FiremakingSkillName);
        var requiredLevel = LogLevelRequirements.GetValueOrDefault(logs.Id, 1);

        if (firemakingSkill.Level.Value < requiredLevel)
        {
            message = $"You need level {requiredLevel} Firemaking to light {logs.Name}.";
            return false;
        }

        // Remove the logs from inventory
        var removed = player.Inventory.Remove(logs);
        if (removed == 0)
        {
            message = "Failed to light the logs.";
            return false;
        }

        // Grant experience
        var experience = LogExperience.GetValueOrDefault(logs.Id, 40);
        Player.AddExperience(firemakingSkill, experience);

        message = $"The fire catches and the logs begin to burn. You gain {experience} Firemaking experience.";
        return true;
    }

    /// <summary>
    ///     Checks if two items can be used together for firemaking.
    /// </summary>
    public static bool CanUseForFiremaking(IItem item1, IItem item2, out IItem? tinderbox, out IItem? logs)
    {
        tinderbox = null;
        logs = null;

        if (IsTinderbox(item1) && IsLog(item2))
        {
            tinderbox = item1;
            logs = item2;
            return true;
        }

        if (IsTinderbox(item2) && IsLog(item1))
        {
            tinderbox = item2;
            logs = item1;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Well-known item IDs for firemaking.
    /// </summary>
    private static class ItemIds
    {
        public static readonly ItemId Tinderbox = new(700);
        public static readonly ItemId Logs = new(1200);
        public static readonly ItemId OakLogs = new(1201);
        public static readonly ItemId WillowLogs = new(1202);
        public static readonly ItemId MapleLogs = new(1203);
        public static readonly ItemId YewLogs = new(1204);
        public static readonly ItemId MagicLogs = new(1205);
    }
}