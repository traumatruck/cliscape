using CliScape.Core.ClueScrolls;
using CliScape.Core.Items;
using CliScape.Core.Items.Actions;

namespace CliScape.Content.Items;

/// <summary>
///     Clue scroll items (one per tier) and reward caskets.
///     Clue scrolls support Read (view current hint) and Use (start trail / attempt step).
///     Reward caskets support Use (open for rewards).
/// </summary>
public static class ClueScrollItems
{
    // Clue Scrolls: 900-903
    public static readonly ActionableItem ClueScrollEasy = CreateClueScroll(
        ItemIds.ClueScrollEasy, "Clue scroll (easy)", ClueScrollTier.Easy,
        "A clue scroll with a simple trail to follow.");

    public static readonly ActionableItem ClueScrollMedium = CreateClueScroll(
        ItemIds.ClueScrollMedium, "Clue scroll (medium)", ClueScrollTier.Medium,
        "A clue scroll with a moderately challenging trail.");

    public static readonly ActionableItem ClueScrollHard = CreateClueScroll(
        ItemIds.ClueScrollHard, "Clue scroll (hard)", ClueScrollTier.Hard,
        "A clue scroll with a difficult trail to follow.");

    public static readonly ActionableItem ClueScrollElite = CreateClueScroll(
        ItemIds.ClueScrollElite, "Clue scroll (elite)", ClueScrollTier.Elite,
        "A clue scroll with an extremely challenging trail.");

    // Reward Caskets: 910-913
    public static readonly ActionableItem RewardCasketEasy = CreateRewardCasket(
        ItemIds.RewardCasketEasy, "Reward casket (easy)", ClueScrollTier.Easy,
        "A casket containing easy treasure trail rewards.");

    public static readonly ActionableItem RewardCasketMedium = CreateRewardCasket(
        ItemIds.RewardCasketMedium, "Reward casket (medium)", ClueScrollTier.Medium,
        "A casket containing medium treasure trail rewards.");

    public static readonly ActionableItem RewardCasketHard = CreateRewardCasket(
        ItemIds.RewardCasketHard, "Reward casket (hard)", ClueScrollTier.Hard,
        "A casket containing hard treasure trail rewards.");

    public static readonly ActionableItem RewardCasketElite = CreateRewardCasket(
        ItemIds.RewardCasketElite, "Reward casket (elite)", ClueScrollTier.Elite,
        "A casket containing elite treasure trail rewards.");

    private static ActionableItem CreateClueScroll(
        ItemId id, string name, ClueScrollTier tier, string examineText)
    {
        var item = new ActionableItem
        {
            Id = id,
            Name = new ItemName(name),
            ExamineText = examineText,
            BaseValue = 0,
            IsStackable = false,
            IsTradeable = false
        };

        item.WithAction(ReadClueAction.Instance);
        item.WithAction(new UseAction(
            "Follow the clue trail",
            (i, player) =>
            {
                if (ClueScrollActions.OnUseClueScroll == null)
                {
                    return "The clue scroll system is not available.";
                }

                return ClueScrollActions.OnUseClueScroll(i, player, tier);
            }));

        return item;
    }

    private static ActionableItem CreateRewardCasket(
        ItemId id, string name, ClueScrollTier tier, string examineText)
    {
        var item = new ActionableItem
        {
            Id = id,
            Name = new ItemName(name),
            ExamineText = examineText,
            BaseValue = 0,
            IsStackable = false,
            IsTradeable = false
        };

        item.WithAction(new UseAction(
            "Open the reward casket",
            (i, player) =>
            {
                if (ClueScrollActions.OnOpenRewardCasket == null)
                {
                    return "The reward system is not available.";
                }

                return ClueScrollActions.OnOpenRewardCasket(i, player, tier);
            },
            consumesItem: true));

        return item;
    }
}
