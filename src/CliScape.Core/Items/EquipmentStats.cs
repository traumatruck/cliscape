namespace CliScape.Core.Items;

/// <summary>
///     Combat statistics for equipment items, mirroring OSRS equipment bonuses.
/// </summary>
public sealed class EquipmentStats
{
    public static readonly EquipmentStats None = new();

    // Attack Bonuses
    public int StabAttackBonus { get; init; }
    public int SlashAttackBonus { get; init; }
    public int CrushAttackBonus { get; init; }
    public int RangedAttackBonus { get; init; }
    public int MagicAttackBonus { get; init; }

    // Defence Bonuses
    public int StabDefenceBonus { get; init; }
    public int SlashDefenceBonus { get; init; }
    public int CrushDefenceBonus { get; init; }
    public int RangedDefenceBonus { get; init; }
    public int MagicDefenceBonus { get; init; }

    // Strength Bonuses
    public int MeleeStrengthBonus { get; init; }
    public int RangedStrengthBonus { get; init; }
    public int MagicDamageBonus { get; init; } // Percentage

    // Other
    public int PrayerBonus { get; init; }

    /// <summary>
    ///     The attack speed modifier for weapons. Lower is faster.
    ///     Standard weapon speed is 4 (2.4 seconds).
    /// </summary>
    public int AttackSpeed { get; init; } = 4;
}