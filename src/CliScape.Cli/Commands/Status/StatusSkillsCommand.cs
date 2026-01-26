using CliScape.Core.Players;
using CliScape.Core.Players.Skills;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Status;

public class StatusSkillsCommand : Command, ICommand
{
    private readonly GameState _gameState = GameState.Instance;

    public static string CommandName => "skills";

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();

        var table = new Table()
            .AddColumn("Skill")
            .AddColumn("Level", col => col.Alignment(Justify.Right))
            .AddColumn("Experience", col => col.Alignment(Justify.Right));

        var attackSkillLevel = player.GetSkillLevel(SkillConstants.AttackSkillName);
        table.AddRow("Attack", $"{attackSkillLevel.Value}", FormatExperience(attackSkillLevel.Experience));

        var strengthSkillLevel = player.GetSkillLevel(SkillConstants.StrengthSkillName);
        table.AddRow("Strength", $"{strengthSkillLevel.Value}", FormatExperience(strengthSkillLevel.Experience));

        var defenceSkillLevel = player.GetSkillLevel(SkillConstants.DefenceSkillName);
        table.AddRow("Defence", $"{defenceSkillLevel.Value}", FormatExperience(defenceSkillLevel.Experience));

        var hitpointsSkillLevel = player.GetSkillLevel(SkillConstants.HitpointsSkillName);
        table.AddRow("Hitpoints", $"{hitpointsSkillLevel.Value}", FormatExperience(hitpointsSkillLevel.Experience));

        var rangedSkillLevel = player.GetSkillLevel(SkillConstants.RangedSkillName);
        table.AddRow("Ranged", $"{rangedSkillLevel.Value}", FormatExperience(rangedSkillLevel.Experience));

        var prayerSkillLevel = player.GetSkillLevel(SkillConstants.PrayerSkillName);
        table.AddRow("Prayer", $"{prayerSkillLevel.Value}", FormatExperience(prayerSkillLevel.Experience));

        var magicSkillLevel = player.GetSkillLevel(SkillConstants.MagicSkillName);
        table.AddRow("Magic", $"{magicSkillLevel.Value}", FormatExperience(magicSkillLevel.Experience));

        var miningSkillLevel = player.GetSkillLevel(SkillConstants.MiningSkillName);
        table.AddRow("Mining", $"{miningSkillLevel.Value}", FormatExperience(miningSkillLevel.Experience));

        var fishingSkillLevel = player.GetSkillLevel(SkillConstants.FishingSkillName);
        table.AddRow("Fishing", $"{fishingSkillLevel.Value}", FormatExperience(fishingSkillLevel.Experience));

        var woodcuttingSkillLevel = player.GetSkillLevel(SkillConstants.WoodcuttingSkillName);
        
        table.AddRow("Woodcutting", $"{woodcuttingSkillLevel.Value}",
            FormatExperience(woodcuttingSkillLevel.Experience));

        var farmingSkillLevel = player.GetSkillLevel(SkillConstants.FarmingSkillName);
        table.AddRow("Farming", $"{farmingSkillLevel.Value}", FormatExperience(farmingSkillLevel.Experience));

        var hunterSkillLevel = player.GetSkillLevel(SkillConstants.HunterSkillName);
        table.AddRow("Hunter", $"{hunterSkillLevel.Value}", FormatExperience(hunterSkillLevel.Experience));

        var smithingSkillLevel = player.GetSkillLevel(SkillConstants.SmithingSkillName);
        table.AddRow("Smithing", $"{smithingSkillLevel.Value}", FormatExperience(smithingSkillLevel.Experience));

        var craftingSkillLevel = player.GetSkillLevel(SkillConstants.CraftingSkillName);
        table.AddRow("Crafting", $"{craftingSkillLevel.Value}", FormatExperience(craftingSkillLevel.Experience));

        var fletchingSkillLevel = player.GetSkillLevel(SkillConstants.FletchingSkillName);
        table.AddRow("Fletching", $"{fletchingSkillLevel.Value}", FormatExperience(fletchingSkillLevel.Experience));

        var cookingSkillLevel = player.GetSkillLevel(SkillConstants.CookingSkillName);
        table.AddRow("Cooking", $"{cookingSkillLevel.Value}", FormatExperience(cookingSkillLevel.Experience));

        var firemakingSkillLevel = player.GetSkillLevel(SkillConstants.FiremakingSkillName);
        table.AddRow("Firemaking", $"{firemakingSkillLevel.Value}", FormatExperience(firemakingSkillLevel.Experience));

        var herbloreSkillLevel = player.GetSkillLevel(SkillConstants.HerbloreSkillName);
        table.AddRow("Herblore", $"{herbloreSkillLevel.Value}", FormatExperience(herbloreSkillLevel.Experience));

        var constructionSkillLevel = player.GetSkillLevel(SkillConstants.ConstructionSkillName);
        
        table.AddRow("Construction", $"{constructionSkillLevel.Value}",
            FormatExperience(constructionSkillLevel.Experience));

        var agilitySkillLevel = player.GetSkillLevel(SkillConstants.AgilitySkillName);
        table.AddRow("Agility", $"{agilitySkillLevel.Value}", FormatExperience(agilitySkillLevel.Experience));

        var thievingSkillLevel = player.GetSkillLevel(SkillConstants.ThievingSkillName);
        table.AddRow("Thieving", $"{thievingSkillLevel.Value}", FormatExperience(thievingSkillLevel.Experience));

        var slayerSkillLevel = player.GetSkillLevel(SkillConstants.SlayerSkillName);
        table.AddRow("Slayer", $"{slayerSkillLevel.Value}", FormatExperience(slayerSkillLevel.Experience));

        var runecraftSkillLevel = player.GetSkillLevel(SkillConstants.RunecraftSkillName);
        table.AddRow("Runecraft", $"{runecraftSkillLevel.Value}", FormatExperience(runecraftSkillLevel.Experience));

        // Total
        table.AddRow("Total Level", player.TotalLevel.ToString(), string.Empty);

        AnsiConsole.Write(table);

        return (int)ExitCode.Success;
    }

    private static string FormatExperience(int experience)
    {
        return experience.ToString("N0");
    }
}