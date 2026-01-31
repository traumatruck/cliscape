namespace CliScape.Core.Npcs;

/// <summary>
/// Base implementation of ICombatableNpc
/// </summary>
public abstract class CombatableNpc : ICombatableNpc
{
    public required NpcName Name { get; init; }
    
    // Basic Combat Info
    public required int CombatLevel { get; init; }
    public required int Hitpoints { get; init; }
    public required int AttackSpeed { get; init; }
    public required bool IsAggressive { get; init; }
    public required bool IsPoisonous { get; init; }
    
    // Combat Stats (visible levels)
    public required int AttackLevel { get; init; }
    public required int StrengthLevel { get; init; }
    public required int DefenceLevel { get; init; }
    public required int RangedLevel { get; init; }
    public required int MagicLevel { get; init; }
    
    // Offensive Bonuses
    public required int StabAttackBonus { get; init; }
    public required int SlashAttackBonus { get; init; }
    public required int CrushAttackBonus { get; init; }
    public required int RangedAttackBonus { get; init; }
    public required int MagicAttackBonus { get; init; }
    
    // Defensive Bonuses
    public required int StabDefenceBonus { get; init; }
    public required int SlashDefenceBonus { get; init; }
    public required int CrushDefenceBonus { get; init; }
    public required int RangedDefenceBonus { get; init; }
    public required int MagicDefenceBonus { get; init; }
    
    // Other Combat Properties
    public required int StrengthBonus { get; init; }
    public required int RangedStrengthBonus { get; init; }
    public required int MagicDamageBonus { get; init; }
    public required int PrayerBonus { get; init; }
    
    // Multiple Attack Styles Support
    public required IReadOnlyList<NpcAttack> Attacks { get; init; }
    
    // Immunities
    public required NpcImmunities Immunities { get; init; }
    
    // Elemental Weakness
    public ElementalWeakness? ElementalWeakness { get; init; }
    
    // Slayer Properties
    public required int SlayerLevel { get; init; }
    public required int SlayerXp { get; init; }
    public string? SlayerCategory { get; init; }

    // Drop Table
    public DropTable DropTable { get; init; } = DropTable.Empty;
}
