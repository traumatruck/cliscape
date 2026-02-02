using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a woodcutting action.
/// </summary>
public record WoodcuttingResult(
    bool Success,
    string Message,
    int LogsObtained,
    string? LogName,
    int TotalExperience,
    LevelUpInfo? LevelUp = null);
