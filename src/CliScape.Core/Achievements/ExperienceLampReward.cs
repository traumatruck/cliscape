using CliScape.Core.Players.Skills;

namespace CliScape.Core.Achievements;

/// <summary>
///     Represents an experience lamp reward that grants XP to a chosen skill.
/// </summary>
public sealed class ExperienceLampReward : DiaryReward
{
    public required Experience ExperienceAmount { get; init; }

    /// <summary>
    ///     Optional filter for which skills can receive the XP.
    ///     If null, any skill can be chosen.
    /// </summary>
    public IReadOnlySet<SkillName>? AllowedSkills { get; init; }

    /// <summary>
    ///     Checks if a skill is allowed to receive this lamp's XP.
    /// </summary>
    public bool IsSkillAllowed(SkillName skill)
    {
        return AllowedSkills == null || AllowedSkills.Contains(skill);
    }
}