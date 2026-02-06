using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands;

public class CommandInterceptor : ICommandInterceptor
{
    private readonly Dictionary<string, int> _skillLevelsBefore = new();
    private bool _hasSkillSnapshot;

    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (IsPersistenceEnabled(settings))
        {
            AnsiConsole.Status().Start("Loading player...", _ => { GameState.Instance.Load(); });
            CaptureSkillLevels();
        }
    }

    public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
    {
        if (!IsPersistenceEnabled(settings))
        {
            return;
        }

        var gameState = GameState.Instance;
        var player = gameState.GetPlayer();

        if (player.CurrentHealth == 0)
        {
            AnsiConsole.WriteLine(MessageConstants.DeathMessage);

            //TODO Death stuff (tele to Lumbridge, lose inventory, etc.)
        }

        DisplayLevelUps(player);

        AnsiConsole.Status().Start("Saving player...", _ => { GameState.Instance.Save(); });
        _hasSkillSnapshot = false;
        _skillLevelsBefore.Clear();
    }

    private static bool IsPersistenceEnabled(CommandSettings settings)
    {
        string[] persistenceDisabledCommands = ["SaveDeleteCommandSettings"];

        return !persistenceDisabledCommands.Contains(settings.GetType().Name);
    }

    private void CaptureSkillLevels()
    {
        var player = GameState.Instance.GetPlayer();

        _skillLevelsBefore.Clear();
        foreach (var skill in player.Skills)
        {
            _skillLevelsBefore[skill.Name.Name] = skill.Level.Value;
        }

        _hasSkillSnapshot = true;
    }

    private void DisplayLevelUps(Player player)
    {
        if (!_hasSkillSnapshot)
        {
            return;
        }

        var wroteHeader = false;

        foreach (var skill in player.Skills)
        {
            if (!_skillLevelsBefore.TryGetValue(skill.Name.Name, out var levelBefore))
            {
                continue;
            }

            var levelAfter = skill.Level.Value;
            if (levelAfter <= levelBefore)
            {
                continue;
            }

            if (!wroteHeader)
            {
                AnsiConsole.WriteLine();
                wroteHeader = true;
            }

            var levelsGained = levelAfter - levelBefore;
            var levelWord = levelsGained == 1 ? "level" : "levels";
            AnsiConsole.MarkupLine(
                $"[bold yellow]Congratulations! Your {skill.Name.Name} level increased by {levelsGained} {levelWord} to {levelAfter}![/]");
        }
    }
}
