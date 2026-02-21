using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Core.Combat;

/// <summary>
///     Default implementation of <see cref="ICombatCalculator" /> using OSRS formulas.
/// </summary>
public sealed class CombatCalculator : ICombatCalculator
{
    /// <summary>
    ///     Singleton instance using the default random provider.
    /// </summary>
    public static readonly CombatCalculator Instance = new(RandomProvider.Instance);

    private readonly IRandomProvider _random;

    /// <summary>
    ///     Creates a new combat calculator with the specified random provider.
    /// </summary>
    /// <param name="random">The random provider to use for hit/damage rolls.</param>
    public CombatCalculator(IRandomProvider random)
    {
        _random = random;
    }

    /// <inheritdoc />
    public int CalculatePlayerMaxHit(Player player)
    {
        var strengthBonus = player.Equipment.TotalMeleeStrengthBonus;
        return CalculatePlayerMaxHit(player, strengthBonus);
    }

    /// <inheritdoc />
    public int CalculatePlayerMaxHit(Player player, int strengthBonus)
    {
        var strengthLevel = player.GetSkill(SkillConstants.StrengthSkillName).Level.Value;

        // Simplified OSRS formula: floor(0.5 + effectiveStrength * (strengthBonus + 64) / 640)
        var effectiveStrength = strengthLevel; // No prayer or potion bonuses for now
        var maxHit = (int)Math.Floor(0.5 + effectiveStrength * (strengthBonus + 64.0) / 640.0);

        return Math.Max(1, maxHit);
    }

    /// <inheritdoc />
    public int CalculatePlayerAttackRoll(Player player)
    {
        // Use the best attack bonus from equipment
        var equipment = player.Equipment;
        var attackBonus = Math.Max(
            Math.Max(equipment.TotalStabAttackBonus, equipment.TotalSlashAttackBonus),
            equipment.TotalCrushAttackBonus);

        return CalculatePlayerAttackRoll(player, attackBonus);
    }

    /// <inheritdoc />
    public int CalculatePlayerAttackRoll(Player player, int attackBonus)
    {
        var attackLevel = player.GetSkill(SkillConstants.AttackSkillName).Level.Value;

        // OSRS formula: (effectiveAttack + 9) * (attackBonus + 64)
        var effectiveAttack = attackLevel + 9;
        return effectiveAttack * (attackBonus + 64);
    }

    /// <inheritdoc />
    public int CalculateNpcDefenceRoll(ICombatableNpc npc)
    {
        // Using crush defence as default for simplicity
        var defenceBonus = npc.CrushDefenceBonus;
        return (npc.DefenceLevel + 9) * (defenceBonus + 64);
    }

    /// <inheritdoc />
    public int CalculateNpcAttackRoll(ICombatableNpc npc)
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

    /// <inheritdoc />
    public int CalculatePlayerDefenceRoll(Player player, ICombatableNpc npc)
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

    /// <inheritdoc />
    public int CalculatePlayerDefenceRoll(Player player, int defenceBonus)
    {
        var defenceLevel = player.GetSkill(SkillConstants.DefenceSkillName).Level.Value;

        // OSRS formula: (effectiveDefence + 9) * (defenceBonus + 64)
        var effectiveDefence = defenceLevel + 9;
        return effectiveDefence * (defenceBonus + 64);
    }

    /// <inheritdoc />
    public bool DoesAttackHit(int attackRoll, int defenceRoll)
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

        return _random.NextDouble() < hitChance;
    }

    /// <inheritdoc />
    public int RollDamage(int maxHit)
    {
        return _random.Next(0, maxHit + 1);
    }

    /// <inheritdoc />
    public CombatExperience CalculateExperience(int damageDealt, CombatStyle style)
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