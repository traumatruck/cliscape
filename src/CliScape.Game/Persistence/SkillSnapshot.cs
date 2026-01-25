namespace CliScape.Game.Persistence;

/// <summary>
///     Represents a snapshot of a skill's state at a point in time.
///     This is used for serialization and persistence of skill data.
/// </summary>
/// <param name="Name">The name of the skill.</param>
/// <param name="Experience">The total experience points in this skill.</param>
public readonly record struct SkillSnapshot(string Name, int Experience);