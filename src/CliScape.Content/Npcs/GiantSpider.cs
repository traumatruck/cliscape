using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Giant Spider NPC found in dungeons.
/// </summary>
public class GiantSpider : CombatableNpc
{
    public static readonly GiantSpider Instance = new()
    {
        Name = new NpcName("Giant spider"),
        CombatLevel = 2,
        Hitpoints = 3,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 1,
        StrengthLevel = 2,
        DefenceLevel = 1,
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
            new NpcAttack(NpcAttackStyle.Stab, 1)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 2,
        SlayerCategory = "Spiders",

        // Drops
        DropTable = DropTable.Empty
    };

    private GiantSpider()
    {
    }
}
