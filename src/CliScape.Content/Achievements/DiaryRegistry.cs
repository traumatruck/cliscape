using System.Reflection;
using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Content.Achievements;

/// <summary>
///     Singleton registry for all achievement diaries in the game.
///     Uses reflection to discover and register diary definitions.
/// </summary>
public sealed class DiaryRegistry
{
    private static readonly Lazy<DiaryRegistry> _instance = new(() => new DiaryRegistry());
    private readonly Dictionary<LocationName, Diary> _diariesByLocation;
    private bool _initialized;

    private DiaryRegistry()
    {
        _diariesByLocation = new Dictionary<LocationName, Diary>();
    }

    public static DiaryRegistry Instance => _instance.Value;

    /// <summary>
    ///     Gets a diary by location name.
    /// </summary>
    public Diary? GetDiary(LocationName location)
    {
        EnsureInitialized();
        _diariesByLocation.TryGetValue(location, out var diary);
        return diary;
    }

    /// <summary>
    ///     Gets all registered diaries.
    /// </summary>
    public IEnumerable<Diary> GetAllDiaries()
    {
        EnsureInitialized();
        return _diariesByLocation.Values;
    }

    /// <summary>
    ///     Initializes the registry by scanning for diary definitions using reflection.
    /// </summary>
    private void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        Initialize();
        _initialized = true;
    }

    private void Initialize()
    {
        // Scan for all types in this assembly that end with "DiaryDefinition"
        var assembly = typeof(DiaryRegistry).Assembly;
        var definitionTypes = assembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("DiaryDefinition"));

        foreach (var type in definitionTypes)
        {
            // Look for a static property named "Diary" of type Diary
            var diaryProperty = type.GetProperty("Diary", BindingFlags.Public | BindingFlags.Static);

            if (diaryProperty?.PropertyType == typeof(Diary))
            {
                var diary = (Diary?)diaryProperty.GetValue(null);
                if (diary != null)
                {
                    _diariesByLocation[diary.Location] = diary;
                }
            }
        }
    }
}