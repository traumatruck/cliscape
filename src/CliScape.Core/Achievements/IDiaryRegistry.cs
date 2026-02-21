using CliScape.Core.World;

namespace CliScape.Core.Achievements;

/// <summary>
///     Provides diary lookups by location without coupling callers to the content layer.
/// </summary>
public interface IDiaryRegistry
{
    /// <summary>
    ///     Gets a diary by its location.
    /// </summary>
    Diary? GetDiary(LocationName location);

    /// <summary>
    ///     Gets all registered diaries.
    /// </summary>
    IEnumerable<Diary> GetAllDiaries();
}
