using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
/// Represents the various Cow NPCs found throughout Gielinor
/// </summary>
public class Cow : CombatableNpc
{
    private Cow() { }
    
    public static readonly Cow Variant1 = new()
    {
        Name = new NpcName("Cow"),
        Variant = new NpcVariant("1", 2),
        CombatLevel = 2,
        Hitpoints = 8,
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
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(
            Poison: true,
            Venom: true
        ),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = "Cows"
    };
    
    public static readonly Cow Variant2 = new()
    {
        Name = new NpcName("Cow"),
        Variant = new NpcVariant("2", 2),
        CombatLevel = 2,
        Hitpoints = 8,
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
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(
            Poison: true,
            Venom: true
        ),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = "Cows"
    };
    
    public static readonly Cow Variant3 = new()
    {
        Name = new NpcName("Cow"),
        Variant = new NpcVariant("3", 2),
        CombatLevel = 2,
        Hitpoints = 8,
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
            new NpcAttack(NpcAttackStyle.Crush, MaxHit: 1)
        },
        
        // Immunities
        Immunities = new NpcImmunities(
            Poison: true,
            Venom: true
        ),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = "Cows"
    };
}
