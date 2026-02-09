using CliScape.Core;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Core.Skills;
using CliScape.Core.World.Resources;

namespace CliScape.Game.Skills;

/// <summary>
///     Default implementation of <see cref="IFishingService" />.
/// </summary>
public sealed class FishingService : IFishingService
{
    public static readonly FishingService Instance = new(
        RandomProvider.Instance,
        ToolChecker.Instance,
        DomainEventDispatcher.Instance);

    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IRandomProvider _random;
    private readonly IToolChecker _toolChecker;

    public FishingService(
        IRandomProvider random,
        IToolChecker toolChecker,
        IDomainEventDispatcher eventDispatcher)
    {
        _random = random;
        _toolChecker = toolChecker;
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public (bool CanFish, string? ErrorMessage) CanFish(Player player, IFishingSpot spot)
    {
        var fishingLevel = player.GetSkillLevel(SkillConstants.FishingSkillName).Value;

        if (fishingLevel < spot.RequiredLevel)
        {
            return (false, $"You need level {spot.RequiredLevel} Fishing to fish here.");
        }

        if (!_toolChecker.HasTool(player, spot.RequiredTool))
        {
            return (false, "You need the required tool to fish here.");
        }

        return (true, null);
    }

    /// <inheritdoc />
    public FishingResult Fish(Player player, IFishingSpot spot, int count, Func<ItemId, IItem?> itemResolver)
    {
        var fishingSkill = player.GetSkill(SkillConstants.FishingSkillName);
        var fishingLevel = fishingSkill.Level.Value;

        // Determine which fish can be caught based on level
        var possibleCatches = spot.PossibleCatches
            .Where(c => fishingLevel >= c.RequiredLevel)
            .ToList();

        if (possibleCatches.Count == 0)
        {
            return new FishingResult(false, "You can't catch anything at this spot yet.", new Dictionary<string, int>(),
                0);
        }

        var fishCaught = new Dictionary<string, int>();
        var totalXp = 0;
        var levelBefore = fishingSkill.Level.Value;

        for (var i = 0; i < count; i++)
        {
            if (player.Inventory.IsFull)
            {
                if (fishCaught.Count == 0)
                {
                    return new FishingResult(false, "Your inventory is full.", new Dictionary<string, int>(), 0);
                }

                break;
            }

            // Pick a random fish
            var caughtFish = possibleCatches[_random.Next(possibleCatches.Count)];

            // Get the fish item
            var fishItem = itemResolver(caughtFish.FishItemId);
            if (fishItem is null)
            {
                continue;
            }

            // Add to inventory
            if (!player.Inventory.TryAdd(fishItem))
            {
                break;
            }

            // Grant experience
            Player.AddExperience(fishingSkill, caughtFish.Experience);
            totalXp += caughtFish.Experience;

            // Track catch count by fish name
            var fishName = fishItem.Name.Value;
            if (!fishCaught.TryAdd(fishName, 1))
            {
                fishCaught[fishName]++;
            }
        }

        if (fishCaught.Count == 0)
        {
            return new FishingResult(false, "You didn't catch anything.", new Dictionary<string, int>(), 0);
        }

        // Check for level up
        LevelUpInfo? levelUp = null;
        var levelAfter = fishingSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            levelUp = new LevelUpInfo(SkillConstants.FishingSkillName, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.FishingSkillName, levelAfter));
        }

        // Raise experience event
        _eventDispatcher.Raise(new ExperienceGainedEvent(SkillConstants.FishingSkillName, totalXp,
            fishingSkill.Level.Experience));

        var catchSummary = string.Join(", ", fishCaught.Select(kvp =>
            kvp.Value == 1 ? kvp.Key : $"{kvp.Value}x {kvp.Key}"));

        return new FishingResult(true, $"You catch: {catchSummary}!", fishCaught, totalXp, levelUp);
    }
}