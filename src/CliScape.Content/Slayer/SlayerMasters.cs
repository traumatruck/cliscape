using CliScape.Core.Slayer;

namespace CliScape.Content.Slayer;

/// <summary>
///     Defines the slayer masters available in the game.
/// </summary>
public static class SlayerMasters
{
    /// <summary>
    ///     Turael, the lowest level slayer master in Burthorpe.
    ///     Assigns easy tasks suitable for beginners.
    /// </summary>
    public static readonly SlayerMaster Turael = new()
    {
        Name = "Turael",
        RequiredCombatLevel = 3,
        RequiredSlayerLevel = 1,
        Assignments =
        [
            new SlayerAssignment { Category = "Birds", MinKills = 10, MaxKills = 20, Weight = 8 },
            new SlayerAssignment { Category = "Cows", MinKills = 10, MaxKills = 20, Weight = 8 },
            new SlayerAssignment { Category = "Goblins", MinKills = 15, MaxKills = 25, Weight = 10 }
        ]
    };

    /// <summary>
    ///     All available slayer masters.
    /// </summary>
    public static IReadOnlyList<SlayerMaster> All => [Turael];

    /// <summary>
    ///     Gets a slayer master by name.
    /// </summary>
    public static SlayerMaster? GetByName(string name)
    {
        return All.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
