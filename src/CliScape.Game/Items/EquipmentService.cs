using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Game.Items;

/// <summary>
///     Default implementation of <see cref="IEquipmentService" />.
/// </summary>
public sealed class EquipmentService : IEquipmentService
{
    public static readonly EquipmentService Instance = new(DomainEventDispatcher.Instance);
    private readonly IDomainEventDispatcher _eventDispatcher;

    public EquipmentService(IDomainEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public (bool CanEquip, string? ErrorMessage) CanEquip(Player player, IEquippable item)
    {
        var skills = player.Skills;

        var attack = GetSkillLevel(skills, SkillConstants.AttackSkillName);
        var strength = GetSkillLevel(skills, SkillConstants.StrengthSkillName);
        var defence = GetSkillLevel(skills, SkillConstants.DefenceSkillName);
        var ranged = GetSkillLevel(skills, SkillConstants.RangedSkillName);
        var magic = GetSkillLevel(skills, SkillConstants.MagicSkillName);

        if (item.RequiredAttackLevel > attack)
        {
            return (false, $"You need {item.RequiredAttackLevel} Attack to equip this.");
        }

        if (item.RequiredStrengthLevel > strength)
        {
            return (false, $"You need {item.RequiredStrengthLevel} Strength to equip this.");
        }

        if (item.RequiredDefenceLevel > defence)
        {
            return (false, $"You need {item.RequiredDefenceLevel} Defence to equip this.");
        }

        if (item.RequiredRangedLevel > ranged)
        {
            return (false, $"You need {item.RequiredRangedLevel} Ranged to equip this.");
        }

        if (item.RequiredMagicLevel > magic)
        {
            return (false, $"You need {item.RequiredMagicLevel} Magic to equip this.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public EquipResult Equip(Player player, IEquippable item)
    {
        var (canEquip, errorMessage) = CanEquip(player, item);
        if (!canEquip)
        {
            return new EquipResult(false, errorMessage!);
        }

        // Remove from inventory
        player.Inventory.Remove(item);

        // Equip the item, getting any previously equipped item back
        var previous = player.Equipment.Equip(item);

        // If there was a previous item, put it in inventory
        if (previous is not null)
        {
            if (!player.Inventory.TryAdd(previous))
            {
                // If inventory is full, this is a problem - we've already removed the new item
                // Put the old item back and restore the new item to inventory
                player.Equipment.Equip(previous);
                player.Inventory.TryAdd(item);
                return new EquipResult(false, "Your inventory is full. Cannot swap equipment.");
            }
        }

        return new EquipResult(true, $"You equip the {item.Name}.", item, previous);
    }

    /// <inheritdoc />
    public EquipResult Unequip(Player player, EquipmentSlot slot)
    {
        var equipped = player.Equipment.GetEquipped(slot);
        if (equipped is null)
        {
            return new EquipResult(false, "Nothing is equipped in that slot.");
        }

        if (player.Inventory.IsFull)
        {
            return new EquipResult(false, "Your inventory is full.");
        }

        player.Equipment.Unequip(slot);
        player.Inventory.TryAdd(equipped);

        return new EquipResult(true, $"You unequip the {equipped.Name}.", null, equipped);
    }

    private static int GetSkillLevel(IPlayerSkill[] skills, SkillName skillName)
    {
        return skills.FirstOrDefault(s => s.Name == skillName)?.Level.Value ?? 1;
    }
}
