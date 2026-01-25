namespace CliScape.Core.Players.Skills;

public class AgilitySkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.AgilitySkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
