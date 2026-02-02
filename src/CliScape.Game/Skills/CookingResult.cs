using CliScape.Core.Skills;

namespace CliScape.Game.Skills;

/// <summary>
///     Result of a cooking action.
/// </summary>
public record CookingResult(
    bool Success,
    string Message,
    int ItemsCooked,
    int ItemsBurnt,
    string? FoodName,
    int TotalExperience,
    LevelUpInfo? LevelUp = null);
