using System.Reflection;
using CliScape.Content.Items.Equippables;
using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Central registry for looking up items by ID or name.
/// </summary>
public static class ItemRegistry
{
    private static readonly Dictionary<ItemId, IItem> ItemsById = new();
    private static readonly Dictionary<string, IItem> ItemsByName = new(StringComparer.OrdinalIgnoreCase);
    private static bool _initialized;

    /// <summary>
    ///     Ensures all items are registered.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        // Auto-discover all items using reflection
        var itemTypes = new[]
        {
            typeof(BronzeEquipment),
            typeof(IronEquipment),
            typeof(SteelEquipment),
            typeof(LeatherEquipment),
            typeof(MiscEquipment),
            typeof(Food),
            typeof(CookedFood),
            typeof(BurntFood),
            typeof(Materials),
            typeof(Tools),
            typeof(Ammunition),
            typeof(Logs),
            typeof(RawFish),
            typeof(Ores),
            typeof(Bars),
            typeof(Pickaxes),
            typeof(ClueScrollItems)
        };

        foreach (var type in itemTypes)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.GetValue(null) is IItem item)
                {
                    ItemsById[item.Id] = item;
                    ItemsByName[item.Name.Value] = item;
                }
            }
        }

        _initialized = true;
    }

    /// <summary>
    ///     Gets an item by its ID.
    /// </summary>
    public static IItem? GetById(ItemId id)
    {
        Initialize();
        return ItemsById.GetValueOrDefault(id);
    }

    /// <summary>
    ///     Gets an item by its name (case-insensitive).
    /// </summary>
    public static IItem? GetByName(string name)
    {
        Initialize();
        return ItemsByName.GetValueOrDefault(name);
    }

    /// <summary>
    ///     Gets all registered items.
    /// </summary>
    public static IEnumerable<IItem> GetAll()
    {
        Initialize();
        return ItemsById.Values;
    }
}