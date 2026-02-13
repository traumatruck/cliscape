using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;

namespace CliScape.Content.Tests;

public class PlayerTests
{
    private static Player CreatePlayer(int currentHealth = SkillConstants.StartingHitpoints,
        int maxHealth = SkillConstants.StartingHitpoints)
    {
        return new Player
        {
            Id = 1,
            Name = "TestPlayer",
            CurrentLocation = new StubLocation(),
            Health = new PlayerHealth { CurrentHealth = currentHealth, MaximumHealth = maxHealth }
        };
    }

    [Fact]
    public void TakeDamage_LessThanHealth_ReducesCorrectly()
    {
        var player = CreatePlayer(10);

        player.TakeDamage(3);

        Assert.Equal(7, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_EqualToHealth_SetsToZero()
    {
        var player = CreatePlayer(10);

        player.TakeDamage(10);

        Assert.Equal(0, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_GreaterThanHealth_ClampsToZero()
    {
        var player = CreatePlayer(5);

        player.TakeDamage(20);

        Assert.Equal(0, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_Zero_NoChange()
    {
        var player = CreatePlayer(10);

        player.TakeDamage(0);

        Assert.Equal(10, player.CurrentHealth);
    }

    [Fact]
    public void Heal_RestoresHealth()
    {
        var player = CreatePlayer(5, 10);

        player.Heal(3);

        Assert.Equal(8, player.CurrentHealth);
    }

    [Fact]
    public void Heal_ClampsToMaximumHealth()
    {
        var player = CreatePlayer(8, 10);

        player.Heal(50);

        Assert.Equal(10, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_ThenHeal_RestoresCorrectly()
    {
        var player = CreatePlayer(10, 10);

        player.TakeDamage(7);
        Assert.Equal(3, player.CurrentHealth);

        player.Heal(4);
        Assert.Equal(7, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_MultipleTimes_AccumulatesCorrectly()
    {
        var player = CreatePlayer(10);

        player.TakeDamage(3);
        player.TakeDamage(3);
        player.TakeDamage(3);

        Assert.Equal(1, player.CurrentHealth);
    }

    [Fact]
    public void TakeDamage_MultipleTimes_NeverGoesNegative()
    {
        var player = CreatePlayer(5);

        player.TakeDamage(3);
        player.TakeDamage(3);
        player.TakeDamage(3);

        Assert.Equal(0, player.CurrentHealth);
    }

    /// <summary>
    ///     Minimal ILocation stub for testing.
    /// </summary>
    private sealed class StubLocation : ILocation
    {
        public static LocationName Name { get; } = new("Test Location");

        public IReadOnlyList<INpc> AvailableNpcs => [];
    }
}