using CliScape.Core.Items.Actions;

namespace CliScape.Core.Items;

/// <summary>
///     A food item that can be eaten to restore hitpoints.
/// </summary>
public class EdibleItem : ActionableItem
{
    /// <summary>
    ///     The amount of hitpoints restored when eating this item.
    /// </summary>
    public required int HealAmount
    {
        get;
        init
        {
            field = value;
            WithAction(new EatAction(value));
        }
    }
}