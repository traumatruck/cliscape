using CliScape.Core.Items;
using CliScape.Core.Players.Skills;

namespace CliScape.Core.Skills;

/// <summary>
///     Result of a skilling action.
/// </summary>
public record SkillingResult(
    bool Success,
    string Message,
    IReadOnlyList<SkillingReward> Rewards,
    LevelUpInfo? LevelUp = null);

/// <summary>
///     A reward from a skilling action.
/// </summary>
public record SkillingReward(ItemId ItemId, string ItemName, int Quantity, int ExperienceGained);

/// <summary>
///     Information about a level-up that occurred.
/// </summary>
public record LevelUpInfo(SkillName SkillName, int NewLevel);