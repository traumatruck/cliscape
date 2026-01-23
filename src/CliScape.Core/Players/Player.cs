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
        if (!location.AdjacentLocations.Values.Contains(CurrentLocation.Name))
        {
            throw new ArgumentException($"Location {CurrentLocation.Name} isn't adjacent to location {location.Name}.");
        }

        CurrentLocation = location;
    }
}