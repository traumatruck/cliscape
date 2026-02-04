namespace CliScape.Core.Achievements;

/// <summary>
///     Value object representing a unique achievement identifier.
/// </summary>
public readonly record struct AchievementId(string Value)
{
    public static implicit operator string(AchievementId id)
    {
        return id.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}