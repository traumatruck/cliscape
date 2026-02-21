namespace CliScape.Core.Slayer;

/// <summary>
///     Wrapper record for slayer category identifiers (e.g., "Goblins", "Demons").
/// </summary>
public sealed record SlayerCategory(string Value)
{
    public override string ToString() => Value;
}
