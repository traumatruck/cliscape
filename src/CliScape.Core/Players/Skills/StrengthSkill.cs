namespace CliScape.Core.Players.Skills;

public class StrengthSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.StrengthSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
