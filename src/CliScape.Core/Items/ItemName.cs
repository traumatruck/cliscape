namespace CliScape.Core.Items;

/// <summary>
///     A wrapper record for item display names.
/// </summary>
public readonly record struct ItemName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}