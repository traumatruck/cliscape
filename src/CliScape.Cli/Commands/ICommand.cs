namespace CliScape.Cli.Commands;

public interface ICommand
{
    static abstract string CommandName { get; }
}