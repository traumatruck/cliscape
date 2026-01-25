namespace CliScape.Core.Players.Skills;

public class MagicSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.MagicSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
