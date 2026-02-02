namespace CliScape.Core.Slayer;

/// <summary>
///     Represents a slayer master who assigns tasks.
/// </summary>
public record SlayerMaster
{
    /// <summary>
    ///     The name of the slayer master.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     The combat level required to receive tasks from this master.
    /// </summary>
    public int RequiredCombatLevel { get; init; }

    /// <summary>
    ///     The slayer level required to receive tasks from this master.
    /// </summary>
    public int RequiredSlayerLevel { get; init; }

    /// <summary>
    ///     The possible task assignments from this master.
    /// </summary>
    public required IReadOnlyList<SlayerAssignment> Assignments { get; init; }
}
