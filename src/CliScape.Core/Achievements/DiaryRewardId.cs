namespace CliScape.Core.Achievements;

/// <summary>
///     Wrapper record for diary reward identifiers (format: "location_tier").
/// </summary>
public sealed record DiaryRewardId(string Value)
{
    public override string ToString() => Value;
}
