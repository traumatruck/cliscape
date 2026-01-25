namespace CliScape.Core.Players.Skills;

public class SmithingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.SmithingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
