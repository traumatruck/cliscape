using CliScape.Core.Items;

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
    ///     Rolls for drops and returns the items that drop.
    /// </summary>
    public IReadOnlyList<DroppedItem> RollDrops()
    {
        var droppedItems = new List<DroppedItem>();

        foreach (var drop in _drops)
        {
            if (ShouldDrop(drop))
            {
                var quantity = RollQuantity(drop);
                droppedItems.Add(new DroppedItem(drop.ItemId, quantity));
            }
        }

        return droppedItems;
    }

    private static bool ShouldDrop(NpcDrop drop)
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
        return Random.Shared.Next(1, dropChance + 1) == 1;
    }

    private static int RollQuantity(NpcDrop drop)
    {
        if (drop.MinQuantity == drop.MaxQuantity)
        {
            return drop.MinQuantity;
        }

        return Random.Shared.Next(drop.MinQuantity, drop.MaxQuantity + 1);
    }

    /// <summary>
    ///     An empty drop table for NPCs with no drops.
    /// </summary>
    public static DropTable Empty => new();
}

/// <summary>
///     Represents an item that has been dropped.
/// </summary>
public record DroppedItem(ItemId ItemId, int Quantity);
