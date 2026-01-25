namespace CliScape.Core.Players.Skills;

public class SlayerSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.SlayerSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
