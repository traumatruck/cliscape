using CliScape.Content.Achievements;
using CliScape.Core.Achievements;
using CliScape.Core.Players;
using CliScape.Core.World;
using CliScape.Game;
using CliScape.Game.Achievements;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Diary;

/// <summary>
///     Command to list all achievement diaries with completion percentages.
/// </summary>
public sealed class DiaryListCommand : Command
{
    public const string CommandName = "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = GameState.Instance.GetPlayer();
        var diaryService = DiaryService.Instance;

        // Check for any newly completed achievements
        var newlyCompleted = diaryService.CheckAllAchievements(player);

        var diaries = DiaryRegistry.Instance.GetAllDiaries().ToList();

        if (diaries.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No achievement diaries are currently available.[/]");
            return (int)ExitCode.Success;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey);

        table.AddColumn(new TableColumn("Location").Centered());
        table.AddColumn(new TableColumn("Easy").Centered());
        table.AddColumn(new TableColumn("Medium").Centered());
        table.AddColumn(new TableColumn("Hard").Centered());
        table.AddColumn(new TableColumn("Elite").Centered());
        table.AddColumn(new TableColumn("Overall").Centered());

        foreach (var diary in diaries.OrderBy(d => d.Location.Value))
        {
            var easyPct = diaryService.GetTierCompletionPercentage(player, diary.Location, DiaryTier.Easy);
            var mediumPct = diaryService.GetTierCompletionPercentage(player, diary.Location, DiaryTier.Medium);
            var hardPct = diaryService.GetTierCompletionPercentage(player, diary.Location, DiaryTier.Hard);
            var elitePct = diaryService.GetTierCompletionPercentage(player, diary.Location, DiaryTier.Elite);
            var overallPct = diaryService.GetOverallCompletionPercentage(player, diary.Location);

            // Check which tiers can be claimed
            var easyClaimable = CanClaimTier(player, diary.Location, DiaryTier.Easy);
            var mediumClaimable = CanClaimTier(player, diary.Location, DiaryTier.Medium);
            var hardClaimable = CanClaimTier(player, diary.Location, DiaryTier.Hard);
            var eliteClaimable = CanClaimTier(player, diary.Location, DiaryTier.Elite);

            table.AddRow(
                diary.Location.Value,
                FormatPercentage(easyPct, easyClaimable),
                FormatPercentage(mediumPct, mediumClaimable),
                FormatPercentage(hardPct, hardClaimable),
                FormatPercentage(elitePct, eliteClaimable),
                FormatPercentage(overallPct, false)
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Use 'diary view <location>' to see achievement details.[/]");
        AnsiConsole.MarkupLine("[grey]Use 'diary claim <location> <tier>' to claim rewards.[/]");

        if (newlyCompleted > 0)
        {
            GameState.Instance.Save();
        }

        return (int)ExitCode.Success;
    }

    private static bool CanClaimTier(Player player, LocationName location, DiaryTier tier)
    {
        return DiaryRewardService.Instance.CanClaimRewards(player, location, tier, out _);
    }

    private static string FormatPercentage(double percentage, bool claimable)
    {
        var pctText = $"{percentage:F0}%";

        if (claimable)
        {
            return $"[green]{pctText} ⭐[/]";
        }

        if (percentage >= 100)
        {
            return $"[green]{pctText}[/]";
        }

        if (percentage > 0)
        {
            return $"[yellow]{pctText}[/]";
        }

        return $"[grey]{pctText}[/]";
    }
}