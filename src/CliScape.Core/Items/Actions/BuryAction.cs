using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Core.Items.Actions;

/// <summary>
///     Action that allows an item to be buried for prayer experience.
/// </summary>
public class BuryAction : IItemAction
{
    /// <summary>
    ///     Creates a new bury action with the specified prayer experience.
    /// </summary>
    /// <param name="prayerExperience">The amount of prayer experience granted.</param>
    public BuryAction(int prayerExperience)
    {
        PrayerExperience = prayerExperience;
    }

    /// <summary>
    ///     The amount of prayer experience gained when burying.
    /// </summary>
    public int PrayerExperience { get; }

    /// <inheritdoc />
    public ItemAction ActionType => ItemAction.Bury;

    /// <inheritdoc />
    public string Description => "Bury for prayer experience";

    /// <inheritdoc />
    public bool ConsumesItem => true;

    /// <inheritdoc />
    public string Execute(IItem item, Player player)
    {
        var prayerSkill = player.GetSkill(SkillConstants.PrayerSkillName);
        Player.AddExperience(prayerSkill, PrayerExperience);
        return $"You bury the {item.Name}. You gain {PrayerExperience} prayer experience.";
    }
}