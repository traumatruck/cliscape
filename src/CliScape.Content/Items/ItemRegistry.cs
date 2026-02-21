using System.Reflection;
using CliScape.Core.Items;

namespace CliScape.Content.Items;

/// <summary>
///     Central registry for looking up items by ID or name.
/// </summary>
public sealed class ItemRegistry : IItemRegistry
{
    public static readonly ItemRegistry Instance = new();

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

        // Auto-discover all types in this assembly that expose public static IItem fields
        var assembly = typeof(ItemRegistry).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
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

    // Explicit interface implementations delegate to the static methods.
    IItem? IItemRegistry.GetById(ItemId id) => GetById(id);
    IItem? IItemRegistry.GetByName(string name) => GetByName(name);
    IEnumerable<IItem> IItemRegistry.GetAll() => GetAll();
}