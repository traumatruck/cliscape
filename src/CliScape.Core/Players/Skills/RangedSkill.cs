namespace CliScape.Core.Players.Skills;

public class RangedSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.RangedSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
