namespace CliScape.Core.Players.Skills;

public class CookingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.CookingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
