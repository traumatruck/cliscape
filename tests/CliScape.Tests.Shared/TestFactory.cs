using CliScape.Core.Items;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.World;
using NSubstitute;

namespace CliScape.Tests.Shared;

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
    ///     Creates a stub IItem via NSubstitute.
    /// </summary>
    public static IItem CreateStubItem(
        ItemId id,
        string name = "Test Item",
        bool stackable = false,
        bool tradeable = true,
        int baseValue = 10)
    {
        var item = Substitute.For<IItem>();
        item.Id.Returns(id);
        item.Name.Returns(new ItemName(name));
        item.IsStackable.Returns(stackable);
        item.IsTradeable.Returns(tradeable);
        item.BaseValue.Returns(baseValue);
        item.ExamineText.Returns($"A {name}.");
        return item;
    }

    /// <summary>
    ///     Creates a stub IEquippable via NSubstitute.
    /// </summary>
    public static IEquippable CreateStubEquippable(
        ItemId id,
        string name,
        EquipmentSlot slot,
        EquipmentStats? stats = null,
        int reqAttack = 1,
        int reqStrength = 1,
        int reqDefence = 1,
        int reqRanged = 1,
        int reqMagic = 1)
    {
        var equip = Substitute.For<IEquippable>();
        equip.Id.Returns(id);
        equip.Name.Returns(new ItemName(name));
        equip.Slot.Returns(slot);
        equip.Stats.Returns(stats ?? EquipmentStats.None);
        equip.RequiredAttackLevel.Returns(reqAttack);
        equip.RequiredStrengthLevel.Returns(reqStrength);
        equip.RequiredDefenceLevel.Returns(reqDefence);
        equip.RequiredRangedLevel.Returns(reqRanged);
        equip.RequiredMagicLevel.Returns(reqMagic);
        equip.IsStackable.Returns(false);
        equip.IsTradeable.Returns(true);
        equip.BaseValue.Returns(10);
        equip.ExamineText.Returns($"A {name}.");
        return equip;
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
