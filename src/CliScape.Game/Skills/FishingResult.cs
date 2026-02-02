using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a fishing action.
/// </summary>
public record FishingResult(
    bool Success,
    string Message,
    IReadOnlyDictionary<string, int> FishCaught,
    int TotalExperience,
    LevelUpInfo? LevelUp = null);
