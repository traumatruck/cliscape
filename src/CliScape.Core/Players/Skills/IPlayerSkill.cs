namespace CliScape.Core.Players.Skills;

public interface IPlayerSkill
{
    SkillName Name { get; }

    PlayerSkillLevel Level { get; set; }
}