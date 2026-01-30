namespace CliScape.Core.Items;

/// <summary>
///     Base implementation for items that can be equipped.
/// </summary>
public class EquippableItem : IEquippable
{
    public required ItemId Id { get; init; }
    
    public required ItemName Name { get; init; }
    
    public required string ExamineText { get; init; }
    
    public required int BaseValue { get; init; }
    
    public bool IsStackable { get; init; }
    
    public bool IsTradeable { get; init; } = true;

    public required EquipmentSlot Slot { get; init; }
    
    public EquipmentStats Stats { get; init; } = EquipmentStats.None;

    public int RequiredAttackLevel { get; init; }
    
    public int RequiredStrengthLevel { get; init; }
    
    public int RequiredDefenceLevel { get; init; }
    
    public int RequiredRangedLevel { get; init; }
    
    public int RequiredMagicLevel { get; init; }
}