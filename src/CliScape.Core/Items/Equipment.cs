namespace CliScape.Core.Items;

/// <summary>
///     The player's equipped items across all 11 OSRS equipment slots.
///     Provides aggregated combat bonuses from all equipped items.
/// </summary>
public sealed class Equipment
{
    private readonly Dictionary<EquipmentSlot, IEquippable> _equipped = new();

    // Aggregated Attack Bonuses
    public int TotalStabAttackBonus => _equipped.Values.Sum(e => e.Stats.StabAttackBonus);
    public int TotalSlashAttackBonus => _equipped.Values.Sum(e => e.Stats.SlashAttackBonus);
    public int TotalCrushAttackBonus => _equipped.Values.Sum(e => e.Stats.CrushAttackBonus);
    public int TotalRangedAttackBonus => _equipped.Values.Sum(e => e.Stats.RangedAttackBonus);
    public int TotalMagicAttackBonus => _equipped.Values.Sum(e => e.Stats.MagicAttackBonus);

    // Aggregated Defence Bonuses
    public int TotalStabDefenceBonus => _equipped.Values.Sum(e => e.Stats.StabDefenceBonus);
    public int TotalSlashDefenceBonus => _equipped.Values.Sum(e => e.Stats.SlashDefenceBonus);
    public int TotalCrushDefenceBonus => _equipped.Values.Sum(e => e.Stats.CrushDefenceBonus);
    public int TotalRangedDefenceBonus => _equipped.Values.Sum(e => e.Stats.RangedDefenceBonus);
    public int TotalMagicDefenceBonus => _equipped.Values.Sum(e => e.Stats.MagicDefenceBonus);

    // Aggregated Strength Bonuses
    public int TotalMeleeStrengthBonus => _equipped.Values.Sum(e => e.Stats.MeleeStrengthBonus);
    public int TotalRangedStrengthBonus => _equipped.Values.Sum(e => e.Stats.RangedStrengthBonus);
    public int TotalMagicDamageBonus => _equipped.Values.Sum(e => e.Stats.MagicDamageBonus);

    // Other
    public int TotalPrayerBonus => _equipped.Values.Sum(e => e.Stats.PrayerBonus);

    /// <summary>
    ///     Gets the attack speed from the equipped weapon, or 4 (default fist speed) if no weapon.
    /// </summary>
    public int WeaponAttackSpeed
    {
        get
        {
            var weapon = GetEquipped(EquipmentSlot.Weapon);
            return weapon?.Stats.AttackSpeed ?? 4;
        }
    }

    /// <summary>
    ///     Gets the item equipped in the specified slot, or null if empty.
    /// </summary>
    public IEquippable? GetEquipped(EquipmentSlot slot)
    {
        return _equipped.GetValueOrDefault(slot);
    }

    /// <summary>
    ///     Equips an item, returning the previously equipped item if any.
    /// </summary>
    public IEquippable? Equip(IEquippable item)
    {
        var previous = GetEquipped(item.Slot);
        _equipped[item.Slot] = item;
        return previous;
    }

    /// <summary>
    ///     Unequips the item in the specified slot, returning it.
    ///     Returns null if the slot was empty.
    /// </summary>
    public IEquippable? Unequip(EquipmentSlot slot)
    {
        if (!_equipped.Remove(slot, out var item))
        {
            return null;
        }

        return item;
    }

    /// <summary>
    ///     Checks if an item is equipped in the specified slot.
    /// </summary>
    public bool IsSlotOccupied(EquipmentSlot slot)
    {
        return _equipped.ContainsKey(slot);
    }

    /// <summary>
    ///     Gets all currently equipped items.
    /// </summary>
    public IEnumerable<IEquippable> GetAllEquipped()
    {
        return _equipped.Values;
    }

    /// <summary>
    ///     Gets all equipped items with their slots.
    /// </summary>
    public IEnumerable<(EquipmentSlot Slot, IEquippable Item)> GetAllEquippedWithSlots()
    {
        return _equipped.Select(kvp => (kvp.Key, kvp.Value));
    }

    /// <summary>
    ///     Clears all equipped items, returning them.
    /// </summary>
    public IReadOnlyList<IEquippable> UnequipAll()
    {
        var items = _equipped.Values.ToList();
        _equipped.Clear();
        return items;
    }
}