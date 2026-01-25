namespace CliScape.Core.Players.Skills;

public class FishingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.FishingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
