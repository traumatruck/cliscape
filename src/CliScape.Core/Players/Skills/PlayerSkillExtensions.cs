namespace CliScape.Core.Players.Skills;

public static class PlayerSkillExtensions
{
    public static int GetLevel(this IPlayerSkill playerSkill)
    {
        return playerSkill.Level.Value;
    }

    public static int GetExperience(this IPlayerSkill playerSkill)
    {
        return playerSkill.Level.Experience;
    }

    /// <summary>
    ///     Adds experience to this skill.
    /// </summary>
    /// <param name="playerSkill">The player skill to be modified.</param>
    /// <param name="experienceGained">The amount of experience to add.</param>
    public static void AddExperience(this IPlayerSkill playerSkill, int experienceGained)
    {
        playerSkill.Level.AddExperience(experienceGained);
    }
}