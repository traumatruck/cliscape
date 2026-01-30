namespace CliScape.Core.Items;

/// <summary>
///     A unique identifier for an item type.
/// </summary>
public readonly record struct ItemId(int Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}