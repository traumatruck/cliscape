namespace CliScape.Core.Slayer;

/// <summary>
///     Provides slayer master lookups without coupling callers to the content layer.
/// </summary>
public interface ISlayerMasterProvider
{
    /// <summary>
    ///     Gets all available slayer masters.
    /// </summary>
    IReadOnlyList<SlayerMaster> All { get; }

    /// <summary>
    ///     Gets a slayer master by their name (case-insensitive).
    /// </summary>
    SlayerMaster? GetByName(string name);
}
