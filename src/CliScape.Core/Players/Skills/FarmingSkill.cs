namespace CliScape.Core.Players.Skills;

public class FarmingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.FarmingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
