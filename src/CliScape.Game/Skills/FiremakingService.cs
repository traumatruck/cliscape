using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a firemaking action.
/// </summary>
public record FiremakingResult(
    bool Success,
    string Message,
    int ExperienceGained,
    LevelUpInfo? LevelUp = null);

/// <summary>
///     Handles firemaking logic.
/// </summary>
public interface IFiremakingService
{
    /// <summary>
    ///     Validates if the player can light the specified logs.
    /// </summary>
    (bool CanLight, string? ErrorMessage) CanLight(Player player, IItem logs);

    /// <summary>
    ///     Attempts to light logs using a tinderbox.
    /// </summary>
    FiremakingResult LightLogs(Player player, IItem logs);

    /// <summary>
    ///     Checks if an item is a type of log that can be burned.
    /// </summary>
    bool IsLog(IItem item);

    /// <summary>
    ///     Checks if an item is a tinderbox.
    /// </summary>
    bool IsTinderbox(IItem item);

    /// <summary>
    ///     Checks if two items can be used together for firemaking.
    /// </summary>
    bool CanUseForFiremaking(IItem item1, IItem item2, out IItem? tinderbox, out IItem? logs);
}

/// <summary>
///     Default implementation of <see cref="IFiremakingService" />.
/// </summary>
public sealed class FiremakingService : IFiremakingService
{
    private static readonly ItemId TinderboxId = new(700);

    private static readonly Dictionary<ItemId, int> LogExperience = new()
    {
        { new ItemId(1200), 40 }, // Logs
        { new ItemId(1201), 60 }, // Oak logs
        { new ItemId(1202), 90 }, // Willow logs
        { new ItemId(1203), 135 }, // Maple logs
        { new ItemId(1204), 203 }, // Yew logs
        { new ItemId(1205), 304 } // Magic logs
    };

    private static readonly Dictionary<ItemId, int> LogLevelRequirements = new()
    {
        { new ItemId(1200), 1 }, // Logs
        { new ItemId(1201), 15 }, // Oak logs
        { new ItemId(1202), 30 }, // Willow logs
        { new ItemId(1203), 45 }, // Maple logs
        { new ItemId(1204), 60 }, // Yew logs
        { new ItemId(1205), 75 } // Magic logs
    };

    public static readonly FiremakingService Instance = new(DomainEventDispatcher.Instance);

    private readonly IDomainEventDispatcher _eventDispatcher;

    public FiremakingService(IDomainEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public bool IsTinderbox(IItem item)
    {
        return item.Id == TinderboxId;
    }

    /// <inheritdoc />
    public bool IsLog(IItem item)
    {
        return LogExperience.ContainsKey(item.Id);
    }

    /// <inheritdoc />
    public (bool CanLight, string? ErrorMessage) CanLight(Player player, IItem logs)
    {
        if (!IsLog(logs))
        {
            return (false, $"You can't light a {logs.Name}.");
        }

        var firemakingSkill = player.GetSkill(SkillConstants.FiremakingSkillName);
        var requiredLevel = LogLevelRequirements.GetValueOrDefault(logs.Id, 1);

        if (firemakingSkill.Level.Value < requiredLevel)
        {
            return (false, $"You need level {requiredLevel} Firemaking to light {logs.Name}.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public FiremakingResult LightLogs(Player player, IItem logs)
    {
        var (canLight, errorMessage) = CanLight(player, logs);
        if (!canLight)
        {
            return new FiremakingResult(false, errorMessage!, 0);
        }

        // Remove the logs from inventory
        var removed = player.Inventory.Remove(logs);
        if (removed == 0)
        {
            return new FiremakingResult(false, "Failed to light the logs.", 0);
        }

        var firemakingSkill = player.GetSkill(SkillConstants.FiremakingSkillName);
        var levelBefore = firemakingSkill.Level.Value;

        // Grant experience
        var experience = LogExperience.GetValueOrDefault(logs.Id, 40);
        Player.AddExperience(firemakingSkill, experience);

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = firemakingSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.FiremakingSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.FiremakingSkillName, levelAfter));
        }

        _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.FiremakingSkillName, experience,
            firemakingSkill.Level.Experience));

        var message = $"The fire catches and the logs begin to burn. You gain {experience} Firemaking experience.";
        return new FiremakingResult(true, message, experience, levelUp);
    }

    /// <inheritdoc />
    public bool CanUseForFiremaking(IItem item1, IItem item2, out IItem? tinderbox, out IItem? logs)
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
}