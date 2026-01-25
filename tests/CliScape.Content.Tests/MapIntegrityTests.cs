using CliScape.Content.Locations.Towns;
using CliScape.Core.World;

namespace CliScape.Content.Tests;

public class MapIntegrityTests
{
    private readonly LocationLibrary _library;

    public MapIntegrityTests()
    {
        _library = new LocationLibrary();
        _library.LoadFrom(typeof(Lumbridge).Assembly);
    }

    [Fact]
    public void AllLocationsHaveReciprocalConnections()
    {
        // Ensure we loaded something
        Assert.NotEmpty(_library.Locations);

        foreach (var (name, location) in _library.Locations)
        {
            foreach (var (direction, neighborName) in location.AdjacentLocations)
            {
                // Assert the neighbor exists
                var neighbor = _library.GetLocation(neighborName);

                if (neighbor == null)
                {
                    Assert.Fail(
                        $"Location '{name.Value}' references unknown neighbor '{neighborName.Value}' to the {direction}.");
                }

                // Assert reciprocal
                var oppositeDir = direction.Opposite();

                if (!neighbor.AdjacentLocations.TryGetValue(oppositeDir, out var neighborTargetName))
                {
                    Assert.Fail(
                        $"Location '{name.Value}' has neighbor '{neighborName.Value}' to {direction}, but '{neighborName.Value}' has no neighbor to {oppositeDir}.");
                }

                if (name != neighborTargetName)
                {
                    Assert.Fail(
                        $"Location '{name.Value}' has neighbor '{neighborName.Value}' to {direction}, but '{neighborName.Value}' points '{oppositeDir}' to '{neighborTargetName.Value}' instead of '{name.Value}'.");
                }
            }
        }
    }
}