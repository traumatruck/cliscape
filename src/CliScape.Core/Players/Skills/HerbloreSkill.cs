namespace CliScape.Core.Players.Skills;

public class HerbloreSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.HerbloreSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
