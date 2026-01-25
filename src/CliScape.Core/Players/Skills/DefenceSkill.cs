namespace CliScape.Core.Players.Skills;

public class DefenceSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.DefenceSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
