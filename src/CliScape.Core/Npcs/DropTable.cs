namespace CliScape.Core.Npcs;

/// <summary>
///     Represents a collection of possible drops from an NPC.
/// </summary>
public class DropTable
{
    private readonly List<NpcDrop> _drops;

    public DropTable(params NpcDrop[] drops)
    {
        _drops = [..drops];
    }

    /// <summary>
    ///     Gets all possible drops.
    /// </summary>
    public IReadOnlyList<NpcDrop> Drops => _drops;

    /// <summary>
    ///     An empty drop table for NPCs with no drops.
    /// </summary>
    public static DropTable Empty => new();

    /// <summary>
    ///     Rolls for drops and returns the items that drop.
    /// </summary>
    /// <param name="random">The random provider to use for drop and quantity rolls.</param>
    public IReadOnlyList<DroppedItem> RollDrops(IRandomProvider random)
    {
        var droppedItems = new List<DroppedItem>();

        foreach (var drop in _drops)
        {
            if (ShouldDrop(drop, random))
            {
                var quantity = RollQuantity(drop, random);
                droppedItems.Add(new DroppedItem(drop.ItemId, quantity));
            }
        }

        return droppedItems;
    }

    private static bool ShouldDrop(NpcDrop drop, IRandomProvider random)
    {
        var dropChance = drop.Rarity switch
        {
            DropRarity.Always => 1,
            DropRarity.Common => 5,
            DropRarity.Uncommon => 25,
            DropRarity.Rare => 100,
            DropRarity.VeryRare => 500,
            DropRarity.Custom => drop.CustomRate,
            _ => 1
        };

        // Roll 1 to dropChance, if we roll 1 we get the drop
        return random.Next(1, dropChance + 1) == 1;
    }

    private static int RollQuantity(NpcDrop drop, IRandomProvider random)
    {
        if (drop.MinQuantity == drop.MaxQuantity)
        {
            return drop.MinQuantity;
        }

        return random.Next(drop.MinQuantity, drop.MaxQuantity + 1);
    }
}