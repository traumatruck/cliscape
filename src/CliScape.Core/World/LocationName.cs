namespace CliScape.Core.World;

public record LocationName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}