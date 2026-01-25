using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands;

public class CommandInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (IsPersistenceEnabled(settings))
        {
            AnsiConsole.Status().Start("Loading player...", _ => { GameState.Instance.Load(); });
        }
    }

    public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
    {
        var gameState = GameState.Instance;
        var player = gameState.GetPlayer();

        if (player.CurrentHealth == 0)
        {
            AnsiConsole.WriteLine(MessageConstants.DeathMessage);

            //TODO Death stuff (tele to Lumbridge, lose inventory, etc.)
        }

        if (IsPersistenceEnabled(settings))
        {
            AnsiConsole.Status().Start("Saving player...", _ => { GameState.Instance.Save(); });
        }
    }

    private static bool IsPersistenceEnabled(CommandSettings settings)
    {
        string[] persistenceDisabledCommands = ["Save"];

        return !persistenceDisabledCommands.Contains(settings.GetType().Name);
    }
}