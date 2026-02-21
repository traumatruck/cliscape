using CliScape.Core.Npcs;
using CliScape.Core.Slayer;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Spider NPC found in dark places.
/// </summary>
public class Spider : CombatableNpc
{
    public static readonly Spider Instance = new()
    {
        Name = new NpcName("Spider"),
        CombatLevel = 1,
        Hitpoints = 1,
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
        StabAttackBonus = -47,
        SlashAttackBonus = -42,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = -42,
        SlashDefenceBonus = -42,
        CrushDefenceBonus = -42,
        RangedDefenceBonus = -42,
        MagicDefenceBonus = -42,

        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Stab, 0)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = new SlayerCategory("Spiders"),

        // Drops
        DropTable = DropTable.Empty
    };

    private Spider()
    {
    }
}