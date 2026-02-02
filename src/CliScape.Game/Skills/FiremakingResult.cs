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
