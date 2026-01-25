namespace CliScape.Core.Players.Skills;

public class ConstructionSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.ConstructionSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
