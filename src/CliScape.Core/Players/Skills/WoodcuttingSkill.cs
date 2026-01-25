namespace CliScape.Core.Players.Skills;

public class WoodcuttingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.WoodcuttingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
