namespace CliScape.Core.Players.Skills;

public class AttackSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.AttackSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}