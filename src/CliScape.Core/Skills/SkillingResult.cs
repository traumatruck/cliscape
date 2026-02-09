namespace CliScape.Core.Skills;

/// <summary>
///     Result of a skilling action.
/// </summary>
public record SkillingResult(
    bool Success,
    string Message,
    IReadOnlyList<SkillingReward> Rewards,
    LevelUpInfo? LevelUp = null);