namespace CliScape.Core.Items;

/// <summary>
///     Base implementation for non-equippable items.
/// </summary>
public class Item : IItem
{
    public required ItemId Id { get; init; }
    
    public required ItemName Name { get; init; }
    
    public required string ExamineText { get; init; }
    
    public required int BaseValue { get; init; }
    
    public bool IsStackable { get; init; }
    
    public bool IsTradeable { get; init; } = true;
}