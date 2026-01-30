using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Core.Combat;

/// <summary>
///     Calculates combat rolls, damage, and experience using OSRS formulas.
/// </summary>
public static class CombatCalculator
{
    /// <summary>
    ///     Calculate player's max hit with melee, automatically using equipment bonuses
    /// </summary>
    public static int CalculatePlayerMaxHit(Player player)
    {
        var strengthBonus = player.Equipment.TotalMeleeStrengthBonus;
        return CalculatePlayerMaxHit(player, strengthBonus);
    }

    /// <summary>
    ///     Calculate player's max hit with melee
    /// </summary>
    public static int CalculatePlayerMaxHit(Player player, int strengthBonus)
    {
        var strengthSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Strength");
        var strengthLevel = strengthSkill?.Level.Value ?? 1;

        // Simplified OSRS formula: floor(0.5 + effectiveStrength * (strengthBonus + 64) / 640)
        var effectiveStrength = strengthLevel; // No prayer or potion bonuses for now
        var maxHit = (int)Math.Floor(0.5 + effectiveStrength * (strengthBonus + 64.0) / 640.0);

        return Math.Max(1, maxHit);
    }

    /// <summary>
    ///     Calculate player's attack roll accuracy, automatically using equipment bonuses.
    ///     Uses the highest offensive bonus from equipped gear.
    /// </summary>
    public static int CalculatePlayerAttackRoll(Player player)
    {
        // Use the best attack bonus from equipment
        var equipment = player.Equipment;
        var attackBonus = Math.Max(
            Math.Max(equipment.TotalStabAttackBonus, equipment.TotalSlashAttackBonus),
            equipment.TotalCrushAttackBonus);

        return CalculatePlayerAttackRoll(player, attackBonus);
    }

    /// <summary>
    ///     Calculate player's attack roll accuracy
    /// </summary>
    public static int CalculatePlayerAttackRoll(Player player, int attackBonus)
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
    ///     Calculate player's defence roll against NPC attack, automatically using equipment bonuses.
    ///     Uses the appropriate defence bonus based on the NPC's attack style.
    /// </summary>
    public static int CalculatePlayerDefenceRoll(Player player, ICombatableNpc npc)
    {
        var attack = npc.Attacks.FirstOrDefault();
        var equipment = player.Equipment;

        var defenceBonus = attack?.Style switch
        {
            NpcAttackStyle.Stab => equipment.TotalStabDefenceBonus,
            NpcAttackStyle.Slash => equipment.TotalSlashDefenceBonus,
            NpcAttackStyle.Crush => equipment.TotalCrushDefenceBonus,
            NpcAttackStyle.Ranged => equipment.TotalRangedDefenceBonus,
            NpcAttackStyle.Magic => equipment.TotalMagicDefenceBonus,
            _ => equipment.TotalCrushDefenceBonus
        };

        return CalculatePlayerDefenceRoll(player, defenceBonus);
    }

    /// <summary>
    ///     Calculate player's defence roll against NPC attack
    /// </summary>
    public static int CalculatePlayerDefenceRoll(Player player, int defenceBonus)
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