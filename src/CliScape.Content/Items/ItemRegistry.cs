using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
/// Central registry for looking up items by ID or name.
/// </summary>
public static class ItemRegistry
{
    private static readonly Dictionary<ItemId, IItem> ItemsById = new();
    private static readonly Dictionary<string, IItem> ItemsByName = new(StringComparer.OrdinalIgnoreCase);
    private static bool _initialized;

    /// <summary>
    /// Ensures all items are registered.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized) return;

        // Register all item categories
        RegisterItems(BronzeEquipment.All);
        RegisterItems(IronEquipment.All);
        RegisterItems(SteelEquipment.All);
        RegisterItems(LeatherEquipment.All);
        RegisterItems(MiscEquipment.All);
        RegisterItems(Food.All);
        RegisterItems(Materials.All);

        _initialized = true;
    }

    private static void RegisterItems(IEnumerable<IItem> items)
    {
        foreach (var item in items)
        {
            ItemsById[item.Id] = item;
            ItemsByName[item.Name.Value] = item;
        }
    }

    /// <summary>
    /// Gets an item by its ID.
    /// </summary>
    public static IItem? GetById(ItemId id)
    {
        Initialize();
        return ItemsById.GetValueOrDefault(id);
    }

    /// <summary>
    /// Gets an item by its name (case-insensitive).
    /// </summary>
    public static IItem? GetByName(string name)
    {
        Initialize();
        return ItemsByName.GetValueOrDefault(name);
    }

    /// <summary>
    /// Gets all registered items.
    /// </summary>
    public static IEnumerable<IItem> GetAll()
    {
        Initialize();
        return ItemsById.Values;
    }
}
