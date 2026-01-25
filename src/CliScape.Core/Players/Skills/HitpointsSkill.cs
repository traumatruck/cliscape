namespace CliScape.Core.Players.Skills;

public class HitpointsSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.HitpointsSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.FromLevel(SkillConstants.StartingHitpoints);
}
