using CliScape.Core.Items;

namespace CliScape.Core.Skills;

/// <summary>
///     A reward from a skilling action.
/// </summary>
public record SkillingReward(ItemId ItemId, string ItemName, int Quantity, int ExperienceGained);
