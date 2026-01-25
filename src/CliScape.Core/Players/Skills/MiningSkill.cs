namespace CliScape.Core.Players.Skills;

public class MiningSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.MiningSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
