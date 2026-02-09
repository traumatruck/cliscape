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
            new SlayerAssignment { Category = "Goblins", MinKills = 15, MaxKills = 25, Weight = 10 },
            new SlayerAssignment { Category = "Rats", MinKills = 10, MaxKills = 20, Weight = 7 },
            new SlayerAssignment { Category = "Spiders", MinKills = 10, MaxKills = 20, Weight = 7 },
            new SlayerAssignment { Category = "Scorpions", MinKills = 10, MaxKills = 15, Weight = 5 },
            new SlayerAssignment { Category = "Skeletons", MinKills = 10, MaxKills = 20, Weight = 6 },
            new SlayerAssignment { Category = "Zombies", MinKills = 10, MaxKills = 20, Weight = 6 },
            new SlayerAssignment { Category = "Hobgoblins", MinKills = 10, MaxKills = 15, Weight = 5 }
        ]
    };

    /// <summary>
    ///     Vannaka, a mid-level slayer master in Edgeville.
    ///     Assigns moderate tasks for experienced adventurers.
    /// </summary>
    public static readonly SlayerMaster Vannaka = new()
    {
        Name = "Vannaka",
        RequiredCombatLevel = 40,
        RequiredSlayerLevel = 1,
        Assignments =
        [
            new SlayerAssignment { Category = "Scorpions", MinKills = 20, MaxKills = 40, Weight = 6 },
            new SlayerAssignment { Category = "Skeletons", MinKills = 20, MaxKills = 40, Weight = 7 },
            new SlayerAssignment { Category = "Zombies", MinKills = 20, MaxKills = 40, Weight = 7 },
            new SlayerAssignment { Category = "Hobgoblins", MinKills = 20, MaxKills = 40, Weight = 8 },
            new SlayerAssignment { Category = "Giants", MinKills = 20, MaxKills = 40, Weight = 10 },
            new SlayerAssignment { Category = "Demons", MinKills = 15, MaxKills = 30, Weight = 5 }
        ]
    };

    /// <summary>
    ///     All available slayer masters.
    /// </summary>
    public static IReadOnlyList<SlayerMaster> All => [Turael, Vannaka];

    /// <summary>
    ///     Gets a slayer master by name.
    /// </summary>
    public static SlayerMaster? GetByName(string name)
    {
        return All.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
