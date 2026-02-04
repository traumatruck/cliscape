namespace CliScape.Core.Achievements;

/// <summary>
///     Abstract base class for diary rewards.
/// </summary>
public abstract class DiaryReward
{
    public required string Description { get; init; }
}