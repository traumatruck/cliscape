using CliScape.Core.Items.Actions;

namespace CliScape.Core.Items;

/// <summary>
///     An item that can be buried for prayer experience.
/// </summary>
public class BuryableItem : ActionableItem
{
    /// <summary>
    ///     The amount of prayer experience gained when burying this item.
    /// </summary>
    public required int PrayerExperience
    {
        get;
        init
        {
            field = value;
            WithAction(new BuryAction(value));
            WithAction(UseAction.Default);
        }
    }
}