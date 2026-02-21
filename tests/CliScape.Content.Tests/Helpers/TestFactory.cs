using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Content.Tests.Helpers;

/// <summary>
///     Factory methods for common test objects.
/// </summary>
public static class TestFactory
{
    /// <summary>
    ///     Creates a player with default stats suitable for testing.
    /// </summary>
    public static Player CreatePlayer(int currentHealth = SkillConstants.StartingHitpoints,
        int maxHealth = SkillConstants.StartingHitpoints)
    {
        return new Player
        {
            Id = 1,
            Name = "TestPlayer",
            CurrentLocation = StubLocation.Instance,
            Health = new PlayerHealth { CurrentHealth = currentHealth, MaximumHealth = maxHealth }
        };
    }

    /// <summary>
    ///     Creates a player with a specific skill set to a given level.
    /// </summary>
    public static Player CreatePlayerWithSkill(SkillName skillName, int level)
    {
        var player = CreatePlayer();
        var skill = player.GetSkill(skillName);
        // Set experience to match the level
        skill.Level = PlayerSkillLevel.FromLevel(level);
        return player;
    }

    /// <summary>
    ///     Minimal ILocation stub for testing.
    /// </summary>
    public sealed class StubLocation : ILocation
    {
        public static readonly StubLocation Instance = new();
        public static LocationName Name { get; } = new("Test Location");
        public IReadOnlyList<INpc> AvailableNpcs => [];
    }
}
