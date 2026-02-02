namespace CliScape.Core.Combat;

/// <summary>
///     Represents a level-up that occurred during combat.
/// </summary>
public record LevelUp(string SkillName, int NewLevel);