using CliScape.Core.Npcs;

namespace CliScape.Content.Npcs;

/// <summary>
/// Represents the Chicken NPC found throughout Gielinor
/// </summary>
public class Chicken : CombatableNpc
{
    private Chicken() { }
    
    public static readonly Chicken Instance = new()
    {
        Name = new NpcName("Chicken"),
        CombatLevel = 1,
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
            new NpcAttack(NpcAttackStyle.Stab, MaxHit: 0)
        },
        
        // Immunities
        Immunities = new NpcImmunities(
            Poison: true,
            Venom: true
        ),
        
        // Slayer
        SlayerLevel = 0,
        SlayerXp = 1,
        SlayerCategory = "Birds"
    };
}
