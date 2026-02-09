using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Hill Giant NPC found in the Edgeville Dungeon
///     and other underground areas. A popular training monster.
/// </summary>
public class HillGiant : CombatableNpc
{
    public static readonly HillGiant Instance = new()
    {
        Name = new NpcName("Hill giant"),
        CombatLevel = 28,
        Hitpoints = 35,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 18,
        StrengthLevel = 22,
        DefenceLevel = 26,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -5,
        SlashDefenceBonus = -5,
        CrushDefenceBonus = -5,
        RangedDefenceBonus = -5,
        MagicDefenceBonus = -5,

        // Other Combat Properties
        StrengthBonus = 7,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 5)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 35,
        SlayerCategory = "Giants",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 10, MaxQuantity = 60, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.IronFullHelm, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.SteelAxe, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.SteelDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.IronOre, MinQuantity = 1, MaxQuantity = 2, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.Coal, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollMedium, Rarity = DropRarity.Rare }
        )
    };

    private HillGiant()
    {
    }
}