using CliScape.Content.Slayer;
using CliScape.Core.Players;
using CliScape.Game;
using CliScape.Game.Slayer;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Slayer;

/// <summary>
///     View and manage slayer tasks.
/// </summary>
public class SlayerCommand(GameState gameState, SlayerService slayerService) : Command<SlayerCommandSettings>, ICommand
{
    public static string CommandName => "slayer";

    public override int Execute(CommandContext context, SlayerCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();

        // Handle cancel option
        if (settings.Cancel)
        {
            return HandleCancel(player);
        }

        // Handle getting a new task
        if (!string.IsNullOrEmpty(settings.Master))
        {
            return HandleGetTask(player, settings.Master);
        }

        // Default: show current task status
        return ShowTaskStatus(player);
    }

    private static int ShowTaskStatus(Player player)
    {
        var task = player.SlayerTask;

        if (task == null || task.IsComplete)
        {
            AnsiConsole.MarkupLine("[yellow]You don't have a slayer task.[/]");
            AnsiConsole.MarkupLine("[dim]Use --master <name> to get a task from a slayer master.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold]Available Slayer Masters:[/]");
            foreach (var master in SlayerMasters.All)
            {
                var requirements = new List<string>();
                if (master.RequiredCombatLevel > 3)
                {
                    requirements.Add($"Combat {master.RequiredCombatLevel}");
                }

                if (master.RequiredSlayerLevel > 1)
                {
                    requirements.Add($"Slayer {master.RequiredSlayerLevel}");
                }

                var reqText = requirements.Count > 0 ? $" [dim](requires {string.Join(", ", requirements)})[/]" : "";
                AnsiConsole.MarkupLine($"  [cyan]{master.Name}[/]{reqText}");
            }

            return (int)ExitCode.Success;
        }

        // Show current task
        var progress = task.TotalKills - task.RemainingKills;
        var percent = (double)progress / task.TotalKills * 100;

        AnsiConsole.MarkupLine("[bold]Current Slayer Task[/]");
        AnsiConsole.MarkupLine($"  [yellow]{task.Category}[/] — {task.RemainingKills} remaining");
        AnsiConsole.MarkupLine($"  Progress: [green]{progress}[/]/{task.TotalKills} ({percent:F0}%)");
        AnsiConsole.MarkupLine($"  [dim]Assigned by {task.SlayerMaster}[/]");

        return (int)ExitCode.Success;
    }

    private int HandleGetTask(Player player, string masterName)
    {
        var master = SlayerMasters.GetByName(masterName);

        if (master == null)
        {
            AnsiConsole.MarkupLine($"[red]Unknown slayer master: {masterName}[/]");
            AnsiConsole.MarkupLine("[dim]Available masters:[/]");
            foreach (var m in SlayerMasters.All)
            {
                AnsiConsole.MarkupLine($"  [cyan]{m.Name}[/]");
            }

            return (int)ExitCode.Failure;
        }

        var result = slayerService.AssignTask(player, master);

        return result switch
        {
            SlayerTaskResult.TaskAssigned assigned => HandleTaskAssigned(assigned.Task, master.Name),
            SlayerTaskResult.AlreadyHaveTask existing => HandleAlreadyHaveTask(existing.CurrentTask),
            SlayerTaskResult.InsufficientCombatLevel combat => HandleInsufficientCombat(combat.Required),
            SlayerTaskResult.InsufficientSlayerLevel slayer => HandleInsufficientSlayer(slayer.Required),
            SlayerTaskResult.NoValidAssignments => HandleNoAssignments(),
            _ => (int)ExitCode.Failure
        };
    }

    private static int HandleTaskAssigned(SlayerTask task, string masterName)
    {
        AnsiConsole.MarkupLine($"[green]{masterName} assigns you a task:[/]");
        AnsiConsole.MarkupLine($"[bold yellow]\"Kill {task.TotalKills} {task.Category}.\"[/]");
        return (int)ExitCode.Success;
    }

    private static int HandleAlreadyHaveTask(SlayerTask task)
    {
        AnsiConsole.MarkupLine("[yellow]You already have a slayer task![/]");
        AnsiConsole.MarkupLine($"  {task.Category} — {task.RemainingKills} remaining");
        AnsiConsole.MarkupLine("[dim]Complete or cancel your current task first (--cancel).[/]");
        return (int)ExitCode.Failure;
    }

    private static int HandleInsufficientCombat(int required)
    {
        AnsiConsole.MarkupLine($"[red]You need combat level {required} to receive tasks from this master.[/]");
        return (int)ExitCode.Failure;
    }

    private static int HandleInsufficientSlayer(int required)
    {
        AnsiConsole.MarkupLine($"[red]You need slayer level {required} to receive tasks from this master.[/]");
        return (int)ExitCode.Failure;
    }

    private static int HandleNoAssignments()
    {
        AnsiConsole.MarkupLine("[red]This slayer master has no tasks suitable for your level.[/]");
        return (int)ExitCode.Failure;
    }

    private int HandleCancel(Player player)
    {
        if (player.SlayerTask == null || player.SlayerTask.IsComplete)
        {
            AnsiConsole.MarkupLine("[yellow]You don't have a slayer task to cancel.[/]");
            return (int)ExitCode.Failure;
        }

        var task = player.SlayerTask;
        slayerService.CancelTask(player);
        AnsiConsole.MarkupLine($"[yellow]Your {task.Category} task has been cancelled.[/]");
        return (int)ExitCode.Success;
    }
}