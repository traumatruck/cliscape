using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Core.Items;

/// <summary>
///     An item that can be buried for prayer experience.
/// </summary>
public class BuryableItem : Item, IBuryable
{
    private static readonly IReadOnlyList<ItemAction> BuryableActions = [ItemAction.Bury];

    /// <inheritdoc />
    public required int PrayerExperience { get; init; }

    /// <inheritdoc />
    public IReadOnlyList<ItemAction> AvailableActions => BuryableActions;

    /// <inheritdoc />
    public bool SupportsAction(ItemAction action) => action == ItemAction.Bury;

    /// <inheritdoc />
    public string Bury(Player player)
    {
        var prayerSkill = player.GetSkill(SkillConstants.PrayerSkillName);
        Player.AddExperience(prayerSkill, PrayerExperience);
        return $"You bury the {Name}. You gain {PrayerExperience} prayer experience.";
    }
}
