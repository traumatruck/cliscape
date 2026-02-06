using CliScape.Content.ClueScrolls;
using CliScape.Content.Items;
using CliScape.Core;
using CliScape.Core.ClueScrolls;
using CliScape.Core.Events;
using CliScape.Core.Items;
using CliScape.Core.Items.Actions;
using CliScape.Core.Players;

namespace CliScape.Game.ClueScrolls;

/// <summary>
///     Wires up the clue scroll system: initializes the service and hooks item action delegates.
/// </summary>
public static class ClueScrollWiring
{
    /// <summary>
    ///     Initializes the clue scroll service and wires the item action delegates.
    ///     Call this once at startup before any commands run.
    /// </summary>
    public static void Initialize()
    {
        var random = RandomProvider.Instance;
        var events = DomainEventDispatcher.Instance;
        var stepPool = ClueStepPool.Instance;
        var rewardTable = ClueRewardTable.Instance;

        ClueScrollService.Initialize(random, events, stepPool, rewardTable);

        ClueScrollActions.OnUseClueScroll = HandleUseClueScroll;
        ClueScrollActions.OnOpenRewardCasket = HandleOpenRewardCasket;
    }

    private static string HandleUseClueScroll(IItem item, Player player, ClueScrollTier tier)
    {
        var service = ClueScrollService.Instance!;

        // If the player has no active clue, start a new trail
        if (player.ActiveClue == null)
        {
            var startResult = service.StartClue(player, tier);
            return startResult switch
            {
                ClueStartResult.Started started =>
                    $"You unroll the clue scroll and begin the treasure trail!\n" +
                    $"[bold]Clue scroll ({started.Clue.Tier}) — Step 1/{started.Clue.TotalSteps}[/]\n" +
                    $"[italic]{started.Clue.CurrentStep.HintText}[/]",

                ClueStartResult.AlreadyHaveClue already =>
                    $"You already have an active {already.CurrentTier} clue scroll to complete.",

                _ => "Something went wrong starting the clue."
            };
        }

        // If the player already has an active clue of a different tier, tell them
        if (player.ActiveClue.Tier != tier)
        {
            return $"You already have an active {player.ActiveClue.Tier} clue scroll. " +
                   "Complete or drop it before starting another.";
        }

        // Attempt to complete the current step at the player's location
        var stepResult = service.AttemptStep(player);
        return stepResult switch
        {
            ClueStepResult.StepCompleted completed =>
                $"{completed.CompletedStep.CompletionText}\n" +
                $"[green]Step {completed.StepNumber}/{completed.TotalSteps} complete![/]\n" +
                $"[bold]Next clue — Step {completed.StepNumber + 1}/{completed.TotalSteps}[/]\n" +
                $"[italic]{completed.NextStep!.HintText}[/]",

            ClueStepResult.ClueCompleted clueCompleted =>
                CompleteClueScroll(player, item, clueCompleted),

            ClueStepResult.WrongLocation wrong =>
                $"This doesn't seem like the right place. " +
                $"You are in [bold]{wrong.CurrentLocation}[/], but the clue points elsewhere.\n" +
                $"[italic]Hint: {player.ActiveClue.CurrentStep.HintText}[/]",

            ClueStepResult.NoActiveClue =>
                "You have no active clue trail. Use the clue scroll to start one.",

            _ => "Something went wrong."
        };
    }

    private static string HandleOpenRewardCasket(IItem item, Player player, ClueScrollTier tier)
    {
        var service = ClueScrollService.Instance!;
        var rewards = service.RollRewards(tier);

        if (rewards.Count == 0)
        {
            return "The casket is empty... how unfortunate.";
        }

        var lines = new List<string>
        {
            $"[bold yellow]You open the {tier} reward casket and find:[/]"
        };

        foreach (var (itemId, quantity) in rewards)
        {
            var rewardItem = ItemRegistry.GetById(itemId);
            if (rewardItem == null)
            {
                continue;
            }

            if (player.Inventory.TryAdd(rewardItem, quantity))
            {
                var quantityText = quantity > 1 ? $" x{quantity}" : "";
                lines.Add($"  [green]• {rewardItem.Name}{quantityText}[/]");
            }
            else
            {
                var quantityText = quantity > 1 ? $" x{quantity}" : "";
                lines.Add($"  [red]• {rewardItem.Name}{quantityText} (no inventory space!)[/]");
            }
        }

        return string.Join("\n", lines);
    }

    private static string CompleteClueScroll(
        Player player, IItem clueScrollItem, ClueStepResult.ClueCompleted result)
    {
        // Remove the clue scroll from inventory since the trail is complete
        player.Inventory.Remove(clueScrollItem);

        return GrantRewardCasket(player, result.Tier);
    }

    private static string GrantRewardCasket(Player player, ClueScrollTier tier)
    {
        var casketId = tier switch
        {
            ClueScrollTier.Easy => ItemIds.RewardCasketEasy,
            ClueScrollTier.Medium => ItemIds.RewardCasketMedium,
            ClueScrollTier.Hard => ItemIds.RewardCasketHard,
            ClueScrollTier.Elite => ItemIds.RewardCasketElite,
            _ => ItemIds.RewardCasketEasy
        };

        var casket = ItemRegistry.GetById(casketId);
        if (casket != null && player.Inventory.TryAdd(casket))
        {
            return $"[bold green]Congratulations! You have completed the {tier} treasure trail![/]\n" +
                   $"A [yellow]{casket.Name}[/] has been added to your inventory.\n" +
                   "Use it to claim your rewards!";
        }

        return $"[bold green]Congratulations! You have completed the {tier} treasure trail![/]\n" +
               "[red]But your inventory is full! Make space and try again.[/]";
    }
}
