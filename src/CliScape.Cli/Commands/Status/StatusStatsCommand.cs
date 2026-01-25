using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

public class StatusStatsCommand : Command, ICommand
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine("Status stats");
        
        throw new NotImplementedException();
    }

    public static string CommandName => "stats";
}