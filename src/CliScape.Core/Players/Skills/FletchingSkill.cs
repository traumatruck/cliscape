namespace CliScape.Core.Players.Skills;

public class FletchingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.FletchingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
