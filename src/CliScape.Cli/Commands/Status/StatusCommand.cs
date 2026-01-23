using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

public class StatusCommand : Command, ICommand
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static string CommandName =>  "status";
}