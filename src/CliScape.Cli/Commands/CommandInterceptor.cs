using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands;

public class CommandInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (settings.GetType().Namespace?.Contains("Save") == true)
        {
            return;
        }

        AnsiConsole.Status().Start("Loading player...", _ => { GameState.Instance.Load(); });
    }

    public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
    {
        if (settings.GetType().Namespace?.Contains("Save") == true)
        {
            return;
        }

        AnsiConsole.Status().Start("Saving player...", _ => { GameState.Instance.Save(); });
    }
}