namespace CliScape.Core.Players.Skills;

public class HunterSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.HunterSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
