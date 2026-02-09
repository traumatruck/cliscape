using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Imp NPC found around Falador and
///     other locations. Small but pesky.
/// </summary>
public class Imp : CombatableNpc
{
    public static readonly Imp Instance = new()
    {
        Name = new NpcName("Imp"),
        CombatLevel = 2,
        Hitpoints = 3,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 1,
        StrengthLevel = 1,
        DefenceLevel = 1,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = -15,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -15,
        SlashDefenceBonus = -15,
        CrushDefenceBonus = -15,
        RangedDefenceBonus = -15,
        MagicDefenceBonus = -15,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, 1)
        },

        // Immunities
        Immunities = new NpcImmunities(),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 0,
        SlayerCategory = null,

        // Drops
        DropTable = new DropTable(
            new NpcDrop { ItemId = ItemIds.Coins, MinQuantity = 1, MaxQuantity = 5, Rarity = DropRarity.Common },
            new NpcDrop { ItemId = ItemIds.Bread, Rarity = DropRarity.Uncommon }
        )
    };

    private Imp()
    {
    }
}
