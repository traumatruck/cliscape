using CliScape.Core;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Players;

namespace CliScape.Game.ClueScrolls;

/// <summary>
///     Service for managing clue scroll interactions: starting, advancing, and completing clues.
/// </summary>
public sealed class ClueScrollService(
    IRandomProvider random,
    IDomainEventDispatcher eventDispatcher,
    IClueStepPool stepPool,
    IClueRewardTable rewardTable)
{
    /// <summary>
    ///     Default singleton instance wired after content is available.
    /// </summary>
    public static ClueScrollService? Instance { get; private set; }

    /// <summary>
    ///     Initializes the singleton instance with the provided dependencies.
    /// </summary>
    public static void Initialize(
        IRandomProvider random,
        IDomainEventDispatcher eventDispatcher,
        IClueStepPool stepPool,
        IClueRewardTable rewardTable)
    {
        Instance = new ClueScrollService(random, eventDispatcher, stepPool, rewardTable);
    }

    /// <summary>
    ///     Starts a new clue scroll for the player by assembling random steps from the pool.
    /// </summary>
    public ClueStartResult StartClue(Player player, ClueScrollTier tier)
    {
        if (player.ActiveClue != null)
        {
            return new ClueStartResult.AlreadyHaveClue(player.ActiveClue.Tier);
        }

        var availableSteps = stepPool.GetSteps(tier);
        if (availableSteps.Count == 0)
        {
            return new ClueStartResult.AlreadyHaveClue(tier);
        }

        var stepCount = GetStepCount(tier);
        var selectedSteps = SelectRandomSteps(availableSteps, stepCount);

        var clue = new ClueScroll
        {
            Tier = tier,
            Steps = selectedSteps,
            CurrentStepIndex = 0
        };

        player.ActiveClue = clue;

        return new ClueStartResult.Started(clue);
    }

    /// <summary>
    ///     Attempts to complete the current clue step at the player's current location.
    /// </summary>
    public ClueStepResult AttemptStep(Player player)
    {
        if (player.ActiveClue == null)
        {
            return new ClueStepResult.NoActiveClue();
        }

        var clue = player.ActiveClue;
        var currentStep = clue.CurrentStep;

        // Validate location
        if (!string.Equals(player.CurrentLocation.Name.Value, currentStep.RequiredLocation.Value,
                StringComparison.OrdinalIgnoreCase))
        {
            return new ClueStepResult.WrongLocation(
                currentStep.RequiredLocation.Value,
                player.CurrentLocation.Name.Value);
        }

        var stepNumber = clue.CurrentStepIndex + 1;
        var totalSteps = clue.TotalSteps;

        // Advance the clue
        var advancedClue = clue.Advance();

        eventDispatcher.Raise(new ClueStepCompletedEvent(clue.Tier, stepNumber, totalSteps));

        // Check if this was the final step
        if (advancedClue.IsComplete)
        {
            player.ActiveClue = null;
            eventDispatcher.Raise(new ClueScrollCompletedEvent(clue.Tier, totalSteps));

            return new ClueStepResult.ClueCompleted(clue.Tier, totalSteps);
        }

        // More steps remain
        player.ActiveClue = advancedClue;

        return new ClueStepResult.StepCompleted(
            currentStep,
            stepNumber,
            totalSteps,
            advancedClue.CurrentStep);
    }

    /// <summary>
    ///     Rolls the reward table and returns the items earned from completing a clue.
    /// </summary>
    public List<(ItemId ItemId, int Quantity)> RollRewards(ClueScrollTier tier)
    {
        var rewards = rewardTable.GetRewards(tier);
        var rollCount = rewardTable.GetRewardRollCount(tier);
        var result = new List<(ItemId, int)>();

        if (rewards.Count == 0)
        {
            return result;
        }

        var totalWeight = rewards.Sum(r => r.Weight);

        for (var i = 0; i < rollCount; i++)
        {
            var roll = random.Next(totalWeight);
            var cumulativeWeight = 0;

            foreach (var reward in rewards)
            {
                cumulativeWeight += reward.Weight;
                if (roll < cumulativeWeight)
                {
                    var quantity = reward.MinQuantity == reward.MaxQuantity
                        ? reward.MinQuantity
                        : random.Next(reward.MinQuantity, reward.MaxQuantity + 1);

                    result.Add((reward.ItemId, quantity));
                    break;
                }
            }
        }

        return result;
    }

    private int GetStepCount(ClueScrollTier tier)
    {
        return tier switch
        {
            ClueScrollTier.Easy => random.Next(2, 4), // 2-3 steps
            ClueScrollTier.Medium => random.Next(3, 5), // 3-4 steps
            ClueScrollTier.Hard => random.Next(4, 6), // 4-5 steps
            ClueScrollTier.Elite => random.Next(5, 7), // 5-6 steps
            _ => 3
        };
    }

    private ClueStep[] SelectRandomSteps(IReadOnlyList<ClueStep> available, int count)
    {
        // Shuffle and pick 'count' steps, avoiding consecutive same-location steps
        var shuffled = available.OrderBy(_ => random.Next(1000)).ToList();
        var selected = new List<ClueStep>();

        foreach (var step in shuffled)
        {
            if (selected.Count >= count)
            {
                break;
            }

            // Avoid back-to-back same location
            if (selected.Count > 0 &&
                selected[^1].RequiredLocation.Value == step.RequiredLocation.Value)
            {
                continue;
            }

            selected.Add(step);
        }

        // If we couldn't fill enough (rare), just take what we can
        while (selected.Count < count && selected.Count < available.Count)
        {
            var step = shuffled[selected.Count % shuffled.Count];
            selected.Add(step);
        }

        return selected.ToArray();
    }
}