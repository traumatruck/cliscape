namespace CliScape.Core.Players.Skills;

/// <summary>
///     A skill that a player can train.
///     This is the primary implementation of <see cref="IPlayerSkill" />.
/// </summary>
public sealed class PlayerSkill : IPlayerSkill
{
    /// <summary>
    ///     Creates a new player skill with the specified name, starting at level 1.
    /// </summary>
    /// <param name="name">The name of the skill.</param>
    public PlayerSkill(SkillName name)
        : this(name, PlayerSkillLevel.CreateNew())
    {
    }

    /// <summary>
    ///     Creates a new player skill with a specific starting level.
    /// </summary>
    /// <param name="name">The name of the skill.</param>
    /// <param name="startingLevel">The starting level for this skill.</param>
    public PlayerSkill(SkillName name, int startingLevel)
        : this(name, PlayerSkillLevel.FromLevel(startingLevel))
    {
    }

    /// <summary>
    ///     Creates a new player skill with the specified name and level.
    /// </summary>
    /// <param name="name">The name of the skill.</param>
    /// <param name="level">The initial level and experience.</param>
    public PlayerSkill(SkillName name, PlayerSkillLevel level)
    {
        Name = name;
        Level = level;
    }

    /// <inheritdoc />
    public SkillName Name { get; }

    /// <inheritdoc />
    public PlayerSkillLevel Level { get; set; }
}