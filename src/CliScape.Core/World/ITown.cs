namespace CliScape.Core.World;

public interface ITown : ILocation
{
    Shop? Shop { get; }

    Bank? Bank { get; }
}