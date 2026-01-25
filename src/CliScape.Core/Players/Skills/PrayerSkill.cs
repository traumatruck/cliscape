namespace CliScape.Core.Players.Skills;

public class PrayerSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.PrayerSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
