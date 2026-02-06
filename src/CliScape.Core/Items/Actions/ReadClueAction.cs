using CliScape.Core.Players;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that reads the current clue hint from the player's active clue scroll.
///     Unlike the static ReadAction, this dynamically displays the current step's hint.
/// </summary>
public class ReadClueAction : IItemAction
{
    public static readonly ReadClueAction Instance = new();

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Read;

    /// <inheritdoc />
    public string Description => "Read the clue scroll";

    /// <inheritdoc />
    public bool ConsumesItem => false;

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        if (player.ActiveClue == null)
        {
            return "You unroll the scroll but it appears blank. Try using the clue scroll to begin the trail.";
        }

        var clue = player.ActiveClue;
        var step = clue.CurrentStep;
        var stepNumber = clue.CurrentStepIndex + 1;

        return $"[bold]Clue scroll ({clue.Tier}) — Step {stepNumber}/{clue.TotalSteps}[/]\n" +
               $"[italic]{step.HintText}[/]";
    }
}