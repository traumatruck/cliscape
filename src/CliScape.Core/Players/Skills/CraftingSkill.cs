namespace CliScape.Core.Players.Skills;

public class CraftingSkill : IPlayerSkill
{
    public SkillName Name => SkillConstants.CraftingSkillName;

    public PlayerSkillLevel Level { get; set; } = PlayerSkillLevel.CreateNew();
}
