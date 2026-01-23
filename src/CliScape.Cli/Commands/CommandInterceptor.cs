using CliScape.Game;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands;

public class CommandInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        Console.WriteLine("Loading...");
        GameState.Instance.Load();
    }
}