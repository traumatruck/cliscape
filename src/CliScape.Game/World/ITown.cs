namespace CliScape.Game.World;

public interface ITown : ILocation
{
    Shop? Shop { get; }

    Bank? Bank { get; }
}