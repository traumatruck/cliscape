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
}