using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a smelting action.
/// </summary>
public record SmeltingResult(
    bool Success,
    string Message,
    int BarsSmelted,
    string? BarName,
    int TotalExperience,
    LevelUpInfo? LevelUp = null);
