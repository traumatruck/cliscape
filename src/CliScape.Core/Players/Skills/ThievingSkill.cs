namespace CliScape.Core.Players.Skills;

public class ThievingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.ThievingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
