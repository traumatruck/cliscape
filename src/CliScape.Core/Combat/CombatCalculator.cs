using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Core.Combat;

/// <summary>
///     Calculates combat rolls, damage, and experience using OSRS formulas.
/// </summary>
public interface ICombatCalculator
{
    /// <summary>
    ///     Calculate player's max hit with melee, automatically using equipment bonuses.
    /// </summary>
    int CalculatePlayerMaxHit(Player player);

    /// <summary>
    ///     Calculate player's max hit with melee using specified strength bonus.
    /// </summary>
    int CalculatePlayerMaxHit(Player player, int strengthBonus);

    /// <summary>
    ///     Calculate player's attack roll accuracy, automatically using equipment bonuses.
    /// </summary>
    int CalculatePlayerAttackRoll(Player player);

    /// <summary>
    ///     Calculate player's attack roll accuracy with specified attack bonus.
    /// </summary>
    int CalculatePlayerAttackRoll(Player player, int attackBonus);

    /// <summary>
    ///     Calculate NPC's defence roll.
    /// </summary>
    int CalculateNpcDefenceRoll(ICombatableNpc npc);

    /// <summary>
    ///     Calculate NPC's attack roll.
    /// </summary>
    int CalculateNpcAttackRoll(ICombatableNpc npc);

    /// <summary>
    ///     Calculate player's defence roll against NPC attack, automatically using equipment bonuses.
    /// </summary>
    int CalculatePlayerDefenceRoll(Player player, ICombatableNpc npc);

    /// <summary>
    ///     Calculate player's defence roll against NPC attack with specified defence bonus.
    /// </summary>
    int CalculatePlayerDefenceRoll(Player player, int defenceBonus);

    /// <summary>
    ///     Determine if an attack hits based on attack and defence rolls.
    /// </summary>
    bool DoesAttackHit(int attackRoll, int defenceRoll);

    /// <summary>
    ///     Roll for damage (0 to maxHit inclusive).
    /// </summary>
    int RollDamage(int maxHit);

    /// <summary>
    ///     Calculate combat experience for dealing damage.
    /// </summary>
    CombatExperience CalculateExperience(int damageDealt, CombatStyle style);
}