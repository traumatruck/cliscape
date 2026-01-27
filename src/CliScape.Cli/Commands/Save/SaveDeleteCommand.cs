using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Save;

public class SaveDeleteCommand : Command<SaveDeleteCommandSettings>, ICommand
{
    public static string CommandName => "delete";

    public override int Execute(CommandContext context, SaveDeleteCommandSettings settings, CancellationToken cancellationToken)
    {
        if (!settings.Force)
        {
            var confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>("Are you sure you want to delete your save file? (y/n)")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(false)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (!confirm)
            {
                AnsiConsole.WriteLine("Save deletion cancelled.");
                return (int)ExitCode.Success;
            }
        }

        var saveFilePath = GameState.Instance.SaveFilePath;

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            AnsiConsole.WriteLine("Save file deleted successfully!");
        }
        else
        {
            AnsiConsole.WriteLine("No save file found to delete.");
        }

        return (int)ExitCode.Success;
    }
}
