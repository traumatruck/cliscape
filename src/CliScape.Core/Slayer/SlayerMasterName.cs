namespace CliScape.Core.Slayer;

/// <summary>
///     Wrapper record for slayer master name identifiers (e.g., "Turael", "Vannaka").
/// </summary>
public sealed record SlayerMasterName(string Value)
{
    public override string ToString() => Value;
}
