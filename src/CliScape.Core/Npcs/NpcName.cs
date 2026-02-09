namespace CliScape.Core.Npcs;

public record NpcName(string Value)
{
    public override string ToString()
    {
        return Value;
    }
}