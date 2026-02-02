namespace CliScape.Core.Combat;

/// <summary>
///     Combat experience breakdown by skill.
/// </summary>
public record CombatExperience(int AttackXp, int StrengthXp, int DefenceXp, int HitpointsXp)
{
    /// <summary>
    ///     Gets the total experience gained across all skills.
    /// </summary>
    public int Total => AttackXp + StrengthXp + DefenceXp + HitpointsXp;

    /// <summary>
    ///     Creates an empty combat experience record.
    /// </summary>
    public static CombatExperience None => new(0, 0, 0, 0);
}