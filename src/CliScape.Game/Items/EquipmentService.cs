using CliScape.Core;
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
    public ServiceResult CanEquip(Player player, IEquippable item)
    {
        var attack = player.GetSkill(SkillConstants.AttackSkillName).Level.Value;
        var strength = player.GetSkill(SkillConstants.StrengthSkillName).Level.Value;
        var defence = player.GetSkill(SkillConstants.DefenceSkillName).Level.Value;
        var ranged = player.GetSkill(SkillConstants.RangedSkillName).Level.Value;
        var magic = player.GetSkill(SkillConstants.MagicSkillName).Level.Value;

        if (item.RequiredAttackLevel > attack)
        {
            return ServiceResult.Fail($"You need {item.RequiredAttackLevel} Attack to equip this.");
        }

        if (item.RequiredStrengthLevel > strength)
        {
            return ServiceResult.Fail($"You need {item.RequiredStrengthLevel} Strength to equip this.");
        }

        if (item.RequiredDefenceLevel > defence)
        {
            return ServiceResult.Fail($"You need {item.RequiredDefenceLevel} Defence to equip this.");
        }

        if (item.RequiredRangedLevel > ranged)
        {
            return ServiceResult.Fail($"You need {item.RequiredRangedLevel} Ranged to equip this.");
        }

        if (item.RequiredMagicLevel > magic)
        {
            return ServiceResult.Fail($"You need {item.RequiredMagicLevel} Magic to equip this.");
        }

        return ServiceResult.Ok();
    }

    /// <inheritdoc />
    public EquipResult Equip(Player player, IEquippable item)
    {
        var canEquipResult = CanEquip(player, item);
        if (!canEquipResult.Success)
        {
            return new EquipResult(false, canEquipResult.Message);
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


}