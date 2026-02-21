using CliScape.Core.Slayer;

namespace CliScape.Content.Slayer;

/// <summary>
///     Defines the slayer masters available in the game.
/// </summary>
public sealed class SlayerMasters : ISlayerMasterProvider
{
    public static readonly SlayerMasters Instance = new();
    /// <summary>
    ///     Turael, the lowest level slayer master in Burthorpe.
    ///     Assigns easy tasks suitable for beginners.
    /// </summary>
    public static readonly SlayerMaster Turael = new()
    {
        Name = new SlayerMasterName("Turael"),
        RequiredCombatLevel = 3,
        RequiredSlayerLevel = 1,
        Assignments =
        [
            new SlayerAssignment { Category = new SlayerCategory("Birds"), MinKills = 10, MaxKills = 20, Weight = 8 },
            new SlayerAssignment { Category = new SlayerCategory("Cows"), MinKills = 10, MaxKills = 20, Weight = 8 },
            new SlayerAssignment { Category = new SlayerCategory("Goblins"), MinKills = 15, MaxKills = 25, Weight = 10 },
            new SlayerAssignment { Category = new SlayerCategory("Rats"), MinKills = 10, MaxKills = 20, Weight = 7 },
            new SlayerAssignment { Category = new SlayerCategory("Spiders"), MinKills = 10, MaxKills = 20, Weight = 7 },
            new SlayerAssignment { Category = new SlayerCategory("Scorpions"), MinKills = 10, MaxKills = 15, Weight = 5 },
            new SlayerAssignment { Category = new SlayerCategory("Skeletons"), MinKills = 10, MaxKills = 20, Weight = 6 },
            new SlayerAssignment { Category = new SlayerCategory("Zombies"), MinKills = 10, MaxKills = 20, Weight = 6 },
            new SlayerAssignment { Category = new SlayerCategory("Hobgoblins"), MinKills = 10, MaxKills = 15, Weight = 5 }
        ]
    };

    /// <summary>
    ///     Vannaka, a mid-level slayer master in Edgeville.
    ///     Assigns moderate tasks for experienced adventurers.
    /// </summary>
    public static readonly SlayerMaster Vannaka = new()
    {
        Name = new SlayerMasterName("Vannaka"),
        RequiredCombatLevel = 40,
        RequiredSlayerLevel = 1,
        Assignments =
        [
            new SlayerAssignment { Category = new SlayerCategory("Scorpions"), MinKills = 20, MaxKills = 40, Weight = 6 },
            new SlayerAssignment { Category = new SlayerCategory("Skeletons"), MinKills = 20, MaxKills = 40, Weight = 7 },
            new SlayerAssignment { Category = new SlayerCategory("Zombies"), MinKills = 20, MaxKills = 40, Weight = 7 },
            new SlayerAssignment { Category = new SlayerCategory("Hobgoblins"), MinKills = 20, MaxKills = 40, Weight = 8 },
            new SlayerAssignment { Category = new SlayerCategory("Giants"), MinKills = 20, MaxKills = 40, Weight = 10 },
            new SlayerAssignment { Category = new SlayerCategory("Demons"), MinKills = 15, MaxKills = 30, Weight = 5 }
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
        return All.FirstOrDefault(m => m.Name.Value.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    // Explicit interface implementations delegate to the static members.
    IReadOnlyList<SlayerMaster> ISlayerMasterProvider.All => All;
    SlayerMaster? ISlayerMasterProvider.GetByName(string name) => GetByName(name);
}