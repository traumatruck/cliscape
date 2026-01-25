using CliScape.Core.World;

namespace CliScape.Core.Players;

public sealed class Player
{
    public required int Id { get; init; }

    public required string Name { get; set; }

    public required ILocation CurrentLocation { get; set; }

    public PlayerHealth Health { get; set; } = new();

    public void Move(ILocation location)
    {
        CurrentLocation = location;
    }
}