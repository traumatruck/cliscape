namespace CliScape.Core.Players.Skills;

public class RunecraftSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.RunecraftSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
