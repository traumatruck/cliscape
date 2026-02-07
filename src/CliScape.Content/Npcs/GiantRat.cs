using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Giant Rat NPC found in swamps and sewers.
/// </summary>
public class GiantRat : CombatableNpc
{
    public static readonly GiantRat Instance = new()
    {
        Name = new NpcName("Giant rat"),
        CombatLevel = 3,
        Hitpoints = 5,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 2,
        StrengthLevel = 3,
        DefenceLevel = 2,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = -21,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -21,
        SlashDefenceBonus = -21,
        CrushDefenceBonus = -21,
        RangedDefenceBonus = -21,
        MagicDefenceBonus = -21,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 2)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 2,
        SlayerCategory = "Rats",

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Bones, Rarity = DropRarity.Always },
            new NpcDrop { ItemId = ItemIds.RawBeef, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 1, MaxQuantity = 5, Rarity = DropRarity.Common }
        )
    };

    private GiantRat()
    {
    }
}
