namespace CliScape.Core.World;

/// <summary>
///     The name of a shop.
/// </summary>
public readonly record struct ShopName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}