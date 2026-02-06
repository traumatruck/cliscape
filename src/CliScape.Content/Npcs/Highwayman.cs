using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Highwayman NPC who attacks players on roads
/// </summary>
public class Highwayman : CombatableNpc
{
    public static readonly Highwayman Instance = new()
    {
        Name = new NpcName("Highwayman"),
        CombatLevel = 5,
        Hitpoints = 13,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 2,
        StrengthLevel = 2,
        DefenceLevel = 2,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 6,
        SlashAttackBonus = 7,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 3,
        CrushDefenceBonus = 2,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 0,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Stab, 1)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 0,
        SlayerCategory = null,

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 5, MaxQuantity = 20, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.BronzeDagger, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.Bread, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollMedium, Rarity = DropRarity.Rare }
        )
    };

    private Highwayman()
    {
    }
}