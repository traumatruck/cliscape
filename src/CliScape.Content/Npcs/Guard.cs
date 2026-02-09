using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Guard NPC found in cities across Gielinor.
/// </summary>
public class Guard : CombatableNpc
{
    public static readonly Guard Instance = new()
    {
        Name = new NpcName("Guard"),
        CombatLevel = 21,
        Hitpoints = 22,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 19,
        StrengthLevel = 18,
        DefenceLevel = 14,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 6,
        SlashAttackBonus = 9,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 11,
        SlashDefenceBonus = 15,
        CrushDefenceBonus = 13,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 0,

        // Other Combat Properties
        StrengthBonus = 7,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Slash, 3)
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
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 10, MaxQuantity = 30, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.IronSword, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.IronChainbody, Rarity = DropRarity.Uncommon },
            new NpcDrop { ItemId = ItemIds.ClueScrollMedium, Rarity = DropRarity.Rare }
        )
    };

    private Guard()
    {
    }
}