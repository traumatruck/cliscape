using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a mining action.
/// </summary>
public record MiningResult(
    bool Success,
    string Message,
    int OresObtained,
    string? OreName,
    int TotalExperience,
    LevelUpInfo? LevelUp = null);