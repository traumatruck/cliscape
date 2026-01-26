namespace CliScape.Core.Npcs;

public interface ICombatableNpc : INpc
{
    // Basic Combat Info
    int CombatLevel { get; }
    int Hitpoints { get; }
    int AttackSpeed { get; } // In game ticks (0.6s per tick)
    bool IsAggressive { get; }
    bool IsPoisonous { get; }
    
    // Combat Stats (visible levels)
    int AttackLevel { get; }
    int StrengthLevel { get; }
    int DefenceLevel { get; }
    int RangedLevel { get; }
    int MagicLevel { get; }
    
    // Offensive Bonuses (Attack Bonuses)
    int StabAttackBonus { get; }
    int SlashAttackBonus { get; }
    int CrushAttackBonus { get; }
    int RangedAttackBonus { get; }
    int MagicAttackBonus { get; }
    
    // Defensive Bonuses (Defence Bonuses)
    int StabDefenceBonus { get; }
    int SlashDefenceBonus { get; }
    int CrushDefenceBonus { get; }
    int RangedDefenceBonus { get; }
    int MagicDefenceBonus { get; }
    
    // Other Combat Properties
    int StrengthBonus { get; } // Melee strength bonus
    int RangedStrengthBonus { get; }
    int MagicDamageBonus { get; } // Percentage bonus
    int PrayerBonus { get; }
    
    // Multiple Attack Styles Support
    IReadOnlyList<NpcAttack> Attacks { get; }
    
    // Immunities
    NpcImmunities Immunities { get; }
    
    // Elemental Weakness (for magic)
    ElementalWeakness? ElementalWeakness { get; }
    
    // Slayer Properties
    int SlayerLevel { get; } // Required slayer level to damage
    int SlayerXp { get; } // XP awarded on kill
    string? SlayerCategory { get; } // e.g., "Goblins", "Demons", etc.
}