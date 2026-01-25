namespace CliScape.Core.Players.Skills;

public class FiremakingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.FiremakingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
