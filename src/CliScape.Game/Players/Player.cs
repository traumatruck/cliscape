using CliScape.Game.World;

namespace CliScape.Game.Players;

public sealed class Player
{
    public required int Id { get; init; }
    
    public required string Name { get; set; }
    
    public ILocation? CurrentLocation { get; set; }
}