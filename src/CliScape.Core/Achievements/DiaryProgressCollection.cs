using CliScape.Core.World;

namespace CliScape.Core.Achievements;

/// <summary>
///     Manages all diary progress for a player.
/// </summary>
public sealed class DiaryProgressCollection
{
    private readonly Dictionary<LocationName, DiaryProgress> _progressByLocation;

    public DiaryProgressCollection()
    {
        _progressByLocation = new Dictionary<LocationName, DiaryProgress>();
    }

    public DiaryProgressCollection(IEnumerable<DiaryProgress> progress)
    {
        _progressByLocation = progress.ToDictionary(p => p.Location);
    }

    /// <summary>
    ///     Gets the progress for a specific location, creating it if it doesn't exist.
    /// </summary>
    public DiaryProgress GetProgress(LocationName location)
    {
        if (!_progressByLocation.TryGetValue(location, out var progress))
        {
            progress = new DiaryProgress(location);
            _progressByLocation[location] = progress;
        }

        return progress;
    }

    /// <summary>
    ///     Gets all diary progress entries.
    /// </summary>
    public IReadOnlyCollection<DiaryProgress> GetAllProgress()
    {
        return _progressByLocation.Values;
    }
}