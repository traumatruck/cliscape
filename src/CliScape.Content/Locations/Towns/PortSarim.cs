using CliScape.Core.Npcs;
using CliScape.Core.World;

namespace CliScape.Content.Locations.Towns;

public class PortSarim : ILocation
{
    public static LocationName Name => new("Port Sarim");

    public Shop? Shop { get; }

    public IReadOnlyList<INpc> AvailableNpcs { get; } = Array.Empty<INpc>();
}