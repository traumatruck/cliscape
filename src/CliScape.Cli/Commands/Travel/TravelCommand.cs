using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel;

public class TravelCommand : Command<TravelCommandSettings>, ICommand
{
    public static string CommandName => "travel";

    public override int Execute(CommandContext context, TravelCommandSettings settings,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}