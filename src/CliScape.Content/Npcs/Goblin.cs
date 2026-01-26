using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
/// Represents the various Goblin NPCs found throughout Gielinor
/// </summary>
public class Goblin : CombatableNpc
{
    private Goblin() { }
    
    public static readonly Goblin Level2 = new()
    {
        Name = new NpcName("Goblin"),
        Variant = new NpcVariant("Level 2", 2),
        CombatLevel = 2,
        Hitpoints = 5,
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
        StabAttackBonus = -21,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = -15,
        
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
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 5,
        SlayerCategory = "Goblins"
    };
    
    public static readonly Goblin Level2Armed = new()
    {
        Name = new NpcName("Goblin"),
        Variant = new NpcVariant("Level 2 (armed)", 2),
        CombatLevel = 2,
        Hitpoints = 5,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,
        
        // Combat Stats
        AttackLevel = 3,
        StrengthLevel = 1,
        DefenceLevel = 4,
        RangedLevel = 1,
        MagicLevel = 1,
        
        // Offensive Bonuses
        StabAttackBonus = -21,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = -15,
        
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
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 5,
        SlayerCategory = "Goblins"
    };
    
    public static readonly Goblin Level5Spear = new()
    {
        Name = new NpcName("Goblin"),
        Variant = new NpcVariant("Level 5", 5),
        CombatLevel = 5,
        Hitpoints = 12,
        AttackSpeed = 6,
        IsAggressive = false,
        IsPoisonous = false,
        
        // Combat Stats
        AttackLevel = 5,
        StrengthLevel = 5,
        DefenceLevel = 1,
        RangedLevel = 1,
        MagicLevel = 1,
        
        // Offensive Bonuses
        StabAttackBonus = 12,
        SlashAttackBonus = -15,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 0,
        
        // Defensive Bonuses
        StabDefenceBonus = 0,
        SlashDefenceBonus = 0,
        CrushDefenceBonus = 0,
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
            new NpcAttack(NpcAttackStyle.Stab, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 12,
        SlayerCategory = "Goblins"
    };
    
    public static readonly Goblin Level13 = new()
    {
        Name = new NpcName("Goblin"),
        Variant = new NpcVariant("Level 13", 13),
        CombatLevel = 13,
        Hitpoints = 16,
        AttackSpeed = 4,
        IsAggressive = false,
        IsPoisonous = false,
        
        // Combat Stats
        AttackLevel = 12,
        StrengthLevel = 13,
        DefenceLevel = 4,
        RangedLevel = 1,
        MagicLevel = 1,
        
        // Offensive Bonuses
        StabAttackBonus = 0,
        SlashAttackBonus = 0,
        CrushAttackBonus = 0,
        RangedAttackBonus = 0,
        MagicAttackBonus = 4,
        
        // Defensive Bonuses
        StabDefenceBonus = 4,
        SlashDefenceBonus = 6,
        CrushDefenceBonus = 8,
        RangedDefenceBonus = 4,
        MagicDefenceBonus = 4,
        
        // Other Combat Properties
        StrengthBonus = 0,
        RangedStrengthBonus = 0,
        MagicDamageBonus = 0,
        PrayerBonus = 0,
        
        // Attacks
        Attacks = new[]
        {
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 2)
        },
        
        // Immunities
        Immunities = new NpcImmunities(),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 16,
        SlayerCategory = "Goblins"
    };
}
