using CliScape.Content.Tests.Helpers;
using CliScape.Core.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Tests;

public class DropTableTests
{
    [Fact]
    public void RollDrops_Empty_ReturnsNoDrops()
    {
        var random = new StubRandomProvider();
        var table = DropTable.Empty;

        var drops = table.RollDrops(random);

        Assert.Empty(drops);
    }

    [Fact]
    public void RollDrops_AlwaysDrop_AlwaysReturnsItem()
    {
        var random = new StubRandomProvider();
        // Always rarity → dropChance = 1 → Next(1, 2) → stub returns default 0, clamped to 1 = 1, so drops
        random.Returns(1);
        var table = new DropTable(
            new NpcDrop { ItemId = new ItemId(100), Rarity = DropRarity.Always }
        );

        var drops = table.RollDrops(random);

        Assert.Single(drops);
        Assert.Equal(new ItemId(100), drops[0].ItemId);
        Assert.Equal(1, drops[0].Quantity);
    }

    [Fact]
    public void RollDrops_CommonDrop_DropsWhenRoll1()
    {
        var random = new StubRandomProvider();
        // Common rarity → dropChance = 5 → Next(1, 6), we queue 1 so it drops
        random.Returns(1);
        var table = new DropTable(
            new NpcDrop { ItemId = new ItemId(200), Rarity = DropRarity.Common }
        );

        var drops = table.RollDrops(random);

        Assert.Single(drops);
        Assert.Equal(new ItemId(200), drops[0].ItemId);
    }

    [Fact]
    public void RollDrops_CommonDrop_DoesNotDropWhenRollNot1()
    {
        var random = new StubRandomProvider();
        // Common rarity → dropChance = 5 → Next(1, 6), we queue 3 so it doesn't drop
        random.Returns(3);
        var table = new DropTable(
            new NpcDrop { ItemId = new ItemId(200), Rarity = DropRarity.Common }
        );

        var drops = table.RollDrops(random);

        Assert.Empty(drops);
    }

    [Fact]
    public void RollDrops_QuantityRange_RollsCorrectQuantity()
    {
        var random = new StubRandomProvider();
        // First call: ShouldDrop check → Always rarity, Next(1,2)=1
        // Second call: RollQuantity → Next(5, 11), we want 7
        random.EnqueueRange(1, 7);
        var table = new DropTable(
            new NpcDrop
            {
                ItemId = new ItemId(300),
                Rarity = DropRarity.Always,
                MinQuantity = 5,
                MaxQuantity = 10
            }
        );

        var drops = table.RollDrops(random);

        Assert.Single(drops);
        Assert.Equal(7, drops[0].Quantity);
    }

    [Fact]
    public void RollDrops_FixedQuantity_NoRandomRoll()
    {
        var random = new StubRandomProvider();
        // Only one random call: ShouldDrop check
        random.Returns(1);
        var table = new DropTable(
            new NpcDrop
            {
                ItemId = new ItemId(400),
                Rarity = DropRarity.Always,
                MinQuantity = 3,
                MaxQuantity = 3
            }
        );

        var drops = table.RollDrops(random);

        Assert.Single(drops);
        Assert.Equal(3, drops[0].Quantity);
    }

    [Fact]
    public void RollDrops_MultipleDrops_CanDropSome()
    {
        var random = new StubRandomProvider();
        // Drop 1: Always → Next(1,2)=1 (drops)
        // Drop 2: Common → Next(1,6), queue 3 (no drop)
        // Drop 3: Always → Next(1,2)=1 (drops)
        random.EnqueueRange(1, 3, 1);
        var table = new DropTable(
            new NpcDrop { ItemId = new ItemId(100), Rarity = DropRarity.Always },
            new NpcDrop { ItemId = new ItemId(200), Rarity = DropRarity.Common },
            new NpcDrop { ItemId = new ItemId(300), Rarity = DropRarity.Always }
        );

        var drops = table.RollDrops(random);

        Assert.Equal(2, drops.Count);
        Assert.Equal(new ItemId(100), drops[0].ItemId);
        Assert.Equal(new ItemId(300), drops[1].ItemId);
    }

    [Fact]
    public void RollDrops_CustomRate_UsesCustomDropChance()
    {
        var random = new StubRandomProvider();
        // Custom rate 50 → Next(1, 51), queue 1 so it drops
        random.Returns(1);
        var table = new DropTable(
            new NpcDrop { ItemId = new ItemId(500), Rarity = DropRarity.Custom, CustomRate = 50 }
        );

        var drops = table.RollDrops(random);

        Assert.Single(drops);
    }
}
