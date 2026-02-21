using CliScape.Core.Players.Skills;

namespace CliScape.Core.Players;

/// <summary>
///     Legacy extension methods — delegate to <see cref="Player.GetSkill(SkillName)" /> and
///     <see cref="Player.GetSkillLevel" /> which use dictionary-backed lookups.
/// </summary>
public static class PlayerExtensions
{
    public static IPlayerSkill GetSkill(this Player player, SkillName skillName)
    {
        return player.GetSkill(skillName);
    }

    public static PlayerSkillLevel GetSkillLevel(this Player player, SkillName skillName)
    {
        return player.GetSkillLevel(skillName);
    }
}