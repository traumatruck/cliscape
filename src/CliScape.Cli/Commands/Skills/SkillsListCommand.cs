using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Skills;

/// <summary>
///     Displays all player skills with levels and experience.
/// </summary>
public class SkillsListCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "list";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();

        var table = new Table()
            .AddColumn("Skill")
            .AddColumn("Level", col => col.Alignment(Justify.Right))
            .AddColumn("Experience", col => col.Alignment(Justify.Right))
            .AddColumn("To Next", col => col.Alignment(Justify.Right));

        // Combat Skills
        AddSkillRow(table, player, SkillConstants.AttackSkillName, "Attack");
        AddSkillRow(table, player, SkillConstants.StrengthSkillName, "Strength");
        AddSkillRow(table, player, SkillConstants.DefenceSkillName, "Defence");
        AddSkillRow(table, player, SkillConstants.HitpointsSkillName, "Hitpoints");
        AddSkillRow(table, player, SkillConstants.RangedSkillName, "Ranged");
        AddSkillRow(table, player, SkillConstants.PrayerSkillName, "Prayer");
        AddSkillRow(table, player, SkillConstants.MagicSkillName, "Magic");

        // Gathering Skills
        AddSkillRow(table, player, SkillConstants.MiningSkillName, "Mining");
        AddSkillRow(table, player, SkillConstants.FishingSkillName, "Fishing");
        AddSkillRow(table, player, SkillConstants.WoodcuttingSkillName, "Woodcutting");
        AddSkillRow(table, player, SkillConstants.FarmingSkillName, "Farming");
        AddSkillRow(table, player, SkillConstants.HunterSkillName, "Hunter");

        // Artisan Skills
        AddSkillRow(table, player, SkillConstants.SmithingSkillName, "Smithing");
        AddSkillRow(table, player, SkillConstants.CraftingSkillName, "Crafting");
        AddSkillRow(table, player, SkillConstants.FletchingSkillName, "Fletching");
        AddSkillRow(table, player, SkillConstants.CookingSkillName, "Cooking");
        AddSkillRow(table, player, SkillConstants.FiremakingSkillName, "Firemaking");
        AddSkillRow(table, player, SkillConstants.HerbloreSkillName, "Herblore");
        AddSkillRow(table, player, SkillConstants.ConstructionSkillName, "Construction");

        // Support Skills
        AddSkillRow(table, player, SkillConstants.AgilitySkillName, "Agility");
        AddSkillRow(table, player, SkillConstants.ThievingSkillName, "Thieving");
        AddSkillRow(table, player, SkillConstants.SlayerSkillName, "Slayer");
        AddSkillRow(table, player, SkillConstants.RunecraftSkillName, "Runecraft");

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"\n[dim]Total Level: {player.TotalLevel} | Combat Level: {player.CombatLevel}[/]");

        return (int)ExitCode.Success;
    }

    private static void AddSkillRow(Table table, Player player, SkillName skillName, string displayName)
    {
        var skillLevel = player.GetSkillLevel(skillName);
        var toNextLevel = skillLevel.Value >= 99 ? "-" : FormatExperience(skillLevel.ExperienceToNextLevel);
        table.AddRow(displayName, $"{skillLevel.Value}", FormatExperience(skillLevel.Experience), toNextLevel);
    }

    private static string FormatExperience(int experience)
    {
        return experience.ToString("N0");
    }
}