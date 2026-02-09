namespace CliScape.Core.World.Resources;

/// <summary>
///     Represents a thieving target in the world that players can steal from.
/// </summary>
public interface IThievingTarget
{
    /// <summary>
    ///     The display name of this thieving target.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The type of thieving target.
    /// </summary>
    ThievingTargetType TargetType { get; }

    /// <summary>
    ///     The minimum thieving level required to steal from this target.
    /// </summary>
    int RequiredLevel { get; }

    /// <summary>
    ///     The thieving experience gained when successfully stealing.
    /// </summary>
    int Experience { get; }

    /// <summary>
    ///     The possible loot that can be obtained from this target.
    /// </summary>
    IReadOnlyList<ThievingLoot> PossibleLoot { get; }

    /// <summary>
    ///     The base success chance at level 1 (0.0 to 1.0).
    ///     Higher levels improve the chance.
    /// </summary>
    double BaseSuccessChance { get; }

    /// <summary>
    ///     The damage taken when caught stealing.
    /// </summary>
    int FailureDamage { get; }
}