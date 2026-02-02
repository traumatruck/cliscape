using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Npcs;

namespace CliScape.Game.Combat;

/// <summary>
///     Result of completing a combat encounter.
/// </summary>
public record CombatOutcomeResult(
    CombatOutcome Outcome,
    IReadOnlyDictionary<string, int> ExperienceGained,
    IReadOnlyList<LevelUp> LevelUps,
    IReadOnlyList<DroppedItem> Drops,
    SlayerTaskProgress? SlayerProgress);
