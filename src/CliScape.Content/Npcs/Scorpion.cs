using CliScape.Content.Items;
using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
///     Represents the Scorpion NPC found in the desert and mines.
/// </summary>
public class Scorpion : CombatableNpc
{
    public static readonly Scorpion Instance = new()
    {
        Name = new NpcName("Scorpion"),
        CombatLevel = 14,
        Hitpoints = 17,
        AttackSpeed = 4,
        IsAggressive = true,
        IsPoisonous = false,

        // Combat Stats
        AttackLevel = 10,
        StrengthLevel = 10,
        DefenceLevel = 10,
        RangedLevel = 1,
        MagicLevel = 1,

        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,

        // Defensive Bonuses
        StabDefenceBonus = 5,
        SlashDefenceBonus = 10,
        CrushDefenceBonus = 5,
        RangedDefenceBonus = 0,
        MagicDefenceBonus = 0,

        // Other Combat Properties
        StrengthBonus = 3,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,

        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Stab, 3)
        },

        // Immunities
        Immunities = new NpcImmunities(
            true,
            true
        ),

        // Slayer
        SlayerLevel = 0,
        SlayerXp = 17,
        SlayerCategory = "Scorpions",

        // Drops
        DropTable = DropTable.Empty
    };

    private Scorpion()
    {
    }
}
