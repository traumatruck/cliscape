using CliScape.Core.Players.Skills;

namespace CliScape.Core.Players;

public static class PlayerExtensions
{
    public static IPlayerSkill GetSkill(this Player player, SkillName skillName)
    {
        return player.Skills.SingleOrDefault(skill => skill.Name == skillName) ??
               throw new InvalidOperationException($"Skill {skillName} not found");
    }

    public static PlayerSkillLevel GetSkillLevel(this Player player, SkillName skillName)
    {
        return player.Skills.SingleOrDefault(skill => skill.Name == skillName)?.Level ??
               throw new InvalidOperationException($"Skill {skillName} not found");
    }
}