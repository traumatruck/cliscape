using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Travel;

public class TravelListCommand: Command, ICommand
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static string CommandName =>  "list";
}