using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Default implementation of <see cref="IWoodcuttingService" />.
/// </summary>
public sealed class WoodcuttingService : IWoodcuttingService
{
    /// <summary>
    ///     Well-known hatchet item IDs in order of effectiveness.
    /// </summary>
    private static readonly ItemId[] HatchetIds =
    [
        new(105) // Bronze hatchet
        // TODO: Add iron, steel, mithril, adamant, rune, dragon hatchets
    ];

    public static readonly WoodcuttingService Instance = new(
        ToolChecker.Instance,
        DomainEventDispatcher.Instance);

    private readonly IDomainEventDispatcher _eventDispatcher;

    private readonly IToolChecker _toolChecker;

    public WoodcuttingService(
        IToolChecker toolChecker,
        IDomainEventDispatcher eventDispatcher)
    {
        _toolChecker = toolChecker;
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public (bool CanChop, string? ErrorMessage) CanChop(Player player, int requiredLevel)
    {
        var woodcuttingLevel = player.GetSkillLevel(SkillConstants.WoodcuttingSkillName).Value;

        if (woodcuttingLevel < requiredLevel)
        {
            return (false, $"You need level {requiredLevel} Woodcutting to chop this tree.");
        }

        if (!_toolChecker.HasAnyTool(player, HatchetIds, out _))
        {
            return (false, "You need a hatchet to chop trees.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public WoodcuttingResult Chop(Player player, ItemId logItemId, int experience, int count,
        Func<ItemId, IItem?> itemResolver)
    {
        var woodcuttingSkill = player.GetSkill(SkillConstants.WoodcuttingSkillName);

        var logItem = itemResolver(logItemId);
        if (logItem is null)
        {
            return new WoodcuttingResult(false, "Something went wrong while chopping.", 0, null, 0);
        }

        var logsObtained = 0;
        var totalXp = 0;
        var levelBefore = woodcuttingSkill.Level.Value;

        for (var i = 0; i < count; i++)
        {
            if (player.Inventory.IsFull)
            {
                if (logsObtained == 0)
                {
                    return new WoodcuttingResult(false, "Your inventory is full.", 0, null, 0);
                }

                break;
            }

            if (!player.Inventory.TryAdd(logItem))
            {
                break;
            }

            Player.AddExperience(woodcuttingSkill, experience);
            logsObtained++;
            totalXp += experience;
        }

        if (logsObtained == 0)
        {
            return new WoodcuttingResult(false, "You didn't get any logs.", 0, null, 0);
        }

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = woodcuttingSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.WoodcuttingSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.WoodcuttingSkillName, levelAfter));
        }

        _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.WoodcuttingSkillName, totalXp,
            woodcuttingSkill.Level.Experience));

        return new WoodcuttingResult(true, $"You get some {logItem.Name}.", logsObtained, logItem.Name.Value, totalXp,
            levelUp);
    }
}
