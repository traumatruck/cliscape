using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Lesser Demon NPC found in Taverly Dungeon
///     and other demonic strongholds.
/// </summary>
public class LesserDemon : CombatableNpc
{
    public static readonly LesserDemon Instance = new()
    {
        Name = new NpcName("Lesser demon"),
        CombatLevel = 82,
        Hitpoints = 79,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 68,
        StrengthLevel = 70,
        DefenceLevel = 65,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = 0,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 0,

        // Other Combat Properties
        StrengthBonus = 12,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Slash, 8),
            new NpcAttack(NpcAttackStyle.Crush, 8)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 79,
        SlayerCategory = "Demons",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 50, MaxQuantity = 300, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.SteelScimitar, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.SteelFullHelm, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.MithrilOre, MinQuantity = 1, MaxQuantity = 2, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.Coal, MinQuantity = 3, MaxQuantity = 5, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.AdamantiteOre, Rarity = DropRarity.Rare },
            new NpcDrop { ItemId = ItemIds.ClueScrollHard, Rarity = DropRarity.Rare }
        )
    };

    private LesserDemon()
    {
    }
}