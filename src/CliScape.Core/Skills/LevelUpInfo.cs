using CliScape.Core.Players.Skills;

namespace CliScape.Core.Skills;

/// <summary>
///     Information about a level-up that occurred.
/// </summary>
public record LevelUpInfo(SkillName SkillName, int NewLevel);
