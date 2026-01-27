using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Core.Combat;

/// <summary>
///     Calculates combat rolls, damage, and experience using OSRS formulas.
/// </summary>
public static class CombatCalculator
{
    /// <summary>
    ///     Calculate player's max hit with melee
    /// </summary>
    public static int CalculatePlayerMaxHit(Player player, int strengthBonus = 0)
    {
        var strengthSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Strength");
        var strengthLevel = strengthSkill?.Level.Value ?? 1;

        // Simplified OSRS formula: floor(0.5 + effectiveStrength * (strengthBonus + 64) / 640)
        var effectiveStrength = strengthLevel; // No prayer or potion bonuses for now
        var maxHit = (int)Math.Floor(0.5 + effectiveStrength * (strengthBonus + 64.0) / 640.0);

        return Math.Max(1, maxHit);
    }

    /// <summary>
    ///     Calculate player's attack roll accuracy
    /// </summary>
    public static int CalculatePlayerAttackRoll(Player player, int attackBonus = 0)
    {
        var attackSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Attack");
        var attackLevel = attackSkill?.Level.Value ?? 1;

        // Simplified: effectiveAttack * (attackBonus + 64)
        var effectiveAttack = attackLevel;
        return effectiveAttack * (attackBonus + 64);
    }

    /// <summary>
    ///     Calculate NPC's defence roll
    /// </summary>
    public static int CalculateNpcDefenceRoll(ICombatableNpc npc)
    {
        // Using crush defence as default for simplicity
        var defenceBonus = npc.CrushDefenceBonus;
        return (npc.DefenceLevel + 9) * (defenceBonus + 64);
    }

    /// <summary>
    ///     Calculate NPC's attack roll
    /// </summary>
    public static int CalculateNpcAttackRoll(ICombatableNpc npc)
    {
        var attack = npc.Attacks.FirstOrDefault();
        var attackBonus = attack?.Style switch
        {
            NpcAttackStyle.Stab => npc.StabAttackBonus,
            NpcAttackStyle.Slash => npc.SlashAttackBonus,
            NpcAttackStyle.Crush => npc.CrushAttackBonus,
            NpcAttackStyle.Ranged => npc.RangedAttackBonus,
            NpcAttackStyle.Magic => npc.MagicAttackBonus,
            _ => npc.CrushAttackBonus
        };

        return (npc.AttackLevel + 9) * (attackBonus + 64);
    }

    /// <summary>
    ///     Calculate player's defence roll against NPC attack
    /// </summary>
    public static int CalculatePlayerDefenceRoll(Player player, int defenceBonus = 0)
    {
        var defenceSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Defence");
        var defenceLevel = defenceSkill?.Level.Value ?? 1;

        return (defenceLevel + 9) * (defenceBonus + 64);
    }

    /// <summary>
    ///     Determine if an attack hits based on attack and defence rolls
    /// </summary>
    public static bool DoesAttackHit(int attackRoll, int defenceRoll)
    {
        double hitChance;

        if (attackRoll > defenceRoll)
        {
            hitChance = 1.0 - (defenceRoll + 2.0) / (2.0 * (attackRoll + 1));
        }
        else
        {
            hitChance = attackRoll / (2.0 * (defenceRoll + 1));
        }

        return Random.Shared.NextDouble() < hitChance;
    }

    /// <summary>
    ///     Roll for damage (0 to maxHit inclusive)
    /// </summary>
    public static int RollDamage(int maxHit)
    {
        return Random.Shared.Next(0, maxHit + 1);
    }

    /// <summary>
    ///     Calculate combat experience for dealing damage
    ///     OSRS gives 4 XP per damage in the attack style skill, plus 1.33 HP XP
    /// </summary>
    public static CombatExperience CalculateExperience(int damageDealt, CombatStyle style)
    {
        var baseXp = damageDealt * 4;
        var hitpointsXp = (int)(damageDealt * 1.33);

        return style switch
        {
            CombatStyle.Accurate => new CombatExperience(baseXp, 0, 0, hitpointsXp),
            CombatStyle.Aggressive => new CombatExperience(0, baseXp, 0, hitpointsXp),
            CombatStyle.Defensive => new CombatExperience(0, 0, baseXp, hitpointsXp),
            CombatStyle.Controlled => new CombatExperience(baseXp / 3, baseXp / 3, baseXp / 3, hitpointsXp),
            _ => new CombatExperience(baseXp, 0, 0, hitpointsXp)
        };
    }
}

/// <summary>
///     Combat experience breakdown
/// </summary>
public record CombatExperience(int AttackXp, int StrengthXp, int DefenceXp, int HitpointsXp);

/// <summary>
///     Combat styles that determine XP distribution
/// </summary>
public enum CombatStyle
{
    Accurate, // Attack XP
    Aggressive, // Strength XP
    Defensive, // Defence XP
    Controlled // Shared XP
}