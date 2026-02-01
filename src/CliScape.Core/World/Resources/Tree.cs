using CliScape.Core.Items;

namespace CliScape.Core.World.Resources;

/// <summary>
///     A standard tree implementation.
/// </summary>
public class Tree : ITree
{
    public required string Name { get; init; }

    public required TreeType TreeType { get; init; }

    public required ItemId LogItemId { get; init; }

    public required int RequiredLevel { get; init; }

    public required int Experience { get; init; }

    public required int MaxActions { get; init; }
}