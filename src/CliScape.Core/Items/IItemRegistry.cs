namespace CliScape.Core.Items;

/// <summary>
///     Provides item lookups by ID or name without coupling callers to the content layer.
/// </summary>
public interface IItemRegistry
{
    /// <summary>
    ///     Gets an item by its unique ID.
    /// </summary>
    IItem? GetById(ItemId id);

    /// <summary>
    ///     Gets an item by its display name (case-insensitive).
    /// </summary>
    IItem? GetByName(string name);

    /// <summary>
    ///     Gets all registered items.
    /// </summary>
    IEnumerable<IItem> GetAll();
}
