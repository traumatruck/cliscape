using System.Reflection;
using CliScape.Core.Achievements;
using CliScape.Core.World;

namespace CliScape.Content.Achievements;

/// <summary>
///     Singleton registry for diary tier rewards.
///     Uses reflection to discover and register reward definitions.
/// </summary>
public sealed class DiaryRewardRegistry : IDiaryRewardRegistry
{
    private static readonly Lazy<DiaryRewardRegistry> _instance = new(() => new DiaryRewardRegistry());
    private readonly Dictionary<(LocationName, DiaryTier), DiaryReward[]> _rewardsByLocationAndTier;
    private bool _initialized;

    private DiaryRewardRegistry()
    {
        _rewardsByLocationAndTier = new Dictionary<(LocationName, DiaryTier), DiaryReward[]>();
    }

    public static DiaryRewardRegistry Instance => _instance.Value;

    /// <summary>
    ///     Gets the rewards for a specific location and tier.
    /// </summary>
    public DiaryReward[] GetRewards(LocationName location, DiaryTier tier)
    {
        EnsureInitialized();
        _rewardsByLocationAndTier.TryGetValue((location, tier), out var rewards);
        return rewards ?? [];
    }

    /// <summary>
    ///     Initializes the registry by scanning for reward definitions using reflection.
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
        // Scan for all types in this assembly that end with "DiaryRewards"
        var assembly = typeof(DiaryRewardRegistry).Assembly;
        var rewardTypes = assembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("DiaryRewards"));

        foreach (var type in rewardTypes)
        {
            // Look for a static method named "GetRewards" with signature:
            // public static Dictionary<DiaryTier, DiaryReward[]> GetRewards(LocationName location)
            var getRewardsMethod = type.GetMethod("GetRewards", BindingFlags.Public | BindingFlags.Static);

            if (getRewardsMethod?.ReturnType == typeof(Dictionary<DiaryTier, DiaryReward[]>))
            {
                // Try to get the Location property to determine which location these rewards belong to
                var locationProperty = type.GetProperty("Location", BindingFlags.Public | BindingFlags.Static);

                if (locationProperty?.PropertyType == typeof(LocationName))
                {
                    var location = (LocationName?)locationProperty.GetValue(null);
                    if (location != null)
                    {
                        var rewards = (Dictionary<DiaryTier, DiaryReward[]>?)getRewardsMethod.Invoke(null, null);
                        if (rewards != null)
                        {
                            foreach (var kvp in rewards)
                            {
                                _rewardsByLocationAndTier[(location, kvp.Key)] = kvp.Value;
                            }
                        }
                    }
                }
            }
        }
    }
}