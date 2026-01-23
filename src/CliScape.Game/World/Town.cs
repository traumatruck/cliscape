namespace CliScape.Game.World;

public class Town : ILocation
{
    public required string Name { get; init; }
    
    public Shop? Shop { get; init; }
    
    public Bank?  Bank { get; init; }
}