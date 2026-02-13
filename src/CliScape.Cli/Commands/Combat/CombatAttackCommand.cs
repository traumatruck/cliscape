using CliScape.Content.Items;
using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Npcs;
using CliScape.Game;
using CliScape.Game.Combat;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Initiates combat with an NPC.
/// </summary>
public class CombatAttackCommand(GameState gameState, ICombatEngine combatEngine)
    : Command<CombatAttackCommandSettings>, ICommand
{
    private string _lastNpcAction = "";
    private string _lastPlayerAction = "";

    public static string CommandName => "attack";

    public override int Execute(CommandContext context, CombatAttackCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if already in combat
        if (gameState.IsInCombat)
        {
            AnsiConsole.MarkupLine("[red]You are already in combat![/]");
            return (int)ExitCode.BadRequest;
        }

        // Find the NPC
        var npc = location.AvailableNpcs
            .OfType<ICombatableNpc>()
            .FirstOrDefault(n => n.Name.Value.Equals(settings.NpcName, StringComparison.OrdinalIgnoreCase));

        if (npc == null)
        {
            AnsiConsole.MarkupLine($"[red]Could not find '{settings.NpcName}' at this location.[/]");
            return (int)ExitCode.BadRequest;
        }

        // Start combat
        var combat = gameState.StartCombat(npc);

        // Run the interactive combat loop
        RunCombatLoop(combat);

        // Process outcome via the engine
        var outcome = combatEngine.ProcessOutcome(combat);
        DisplayOutcome(combat, npc, outcome);

        gameState.EndCombat();

        return (int)ExitCode.Success;
    }

    private void RunCombatLoop(CombatSession combat)
    {
        while (!combat.IsComplete)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(BuildCombatDisplay(combat));

            var action = ShowCombatMenu();

            switch (action)
            {
                case CombatAction.Attack:
                    var combatStyle = ChooseCombatStyle();
                    var turnResult = combatEngine.ExecuteTurn(combat, combatStyle);
                    FormatTurnResult(combat.Npc, turnResult);
                    break;

                case CombatAction.Inventory:
                    _lastPlayerAction = "[yellow]Your inventory is empty.[/]";
                    _lastNpcAction = "";
                    continue;

                case CombatAction.Magic:
                    _lastPlayerAction = "[blue]You don't know any spells yet.[/]";
                    _lastNpcAction = "";
                    continue;

                case CombatAction.Run:
                    var fleeResult = combatEngine.AttemptFlee(combat);
                    if (fleeResult.Success)
                    {
                        _lastPlayerAction = "[green]Got away safely![/]";
                        _lastNpcAction = "";
                        AnsiConsole.Clear();
                        AnsiConsole.Write(BuildCombatDisplay(combat));
                        return;
                    }

                    _lastPlayerAction = "[red]Can't escape![/]";
                    _lastNpcAction = "";
                    break;
            }
        }

        // Show final combat state
        AnsiConsole.Clear();
        AnsiConsole.Write(BuildCombatDisplay(combat));
    }

    private void FormatTurnResult(ICombatableNpc npc, CombatTurnResult result)
    {
        // Format player attack
        var pa = result.PlayerAttack;
        
        if (pa is { Hit: true, Damage: > 0 })
        {
            _lastPlayerAction = $"[green]You hit the {npc.Name} for {pa.Damage} damage![/]";
        }
        else if (pa.Hit)
        {
            _lastPlayerAction = $"[dim]You hit the {npc.Name} but deal no damage.[/]";
        }
        else
        {
            _lastPlayerAction = $"[dim]Your attack misses the {npc.Name}.[/]";
        }

        // Format NPC attack
        if (result.NpcAttack is { } na)
        {
            if (na is { Hit: true, Damage: > 0 })
            {
                _lastNpcAction = $"[red]The {npc.Name} hits you for {na.Damage} damage![/]";
            }
            else if (na.Hit)
            {
                _lastNpcAction = $"[dim]The {npc.Name} hits you but deals no damage.[/]";
            }
            else
            {
                _lastNpcAction = $"[dim]The {npc.Name}'s attack misses you.[/]";
            }
        }
        else
        {
            _lastNpcAction = "";
        }
    }

    private void DisplayOutcome(CombatSession combat, ICombatableNpc npc, CombatOutcomeResult outcome)
    {
        switch (outcome.Outcome)
        {
            case CombatOutcome.PlayerVictory:
                AnsiConsole.MarkupLine($"[bold green]You defeated the {npc.Name}![/]");

                // Display XP summary
                if (outcome.ExperienceGained.Count > 0)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[bold]Experience gained:[/]");
                    foreach (var (skillName, xp) in outcome.ExperienceGained)
                    {
                        AnsiConsole.MarkupLine($"  [cyan]{skillName}[/]: [white]+{xp} XP[/]");
                    }
                }

                // Display level-ups
                foreach (var levelUp in outcome.LevelUps)
                {
                    AnsiConsole.MarkupLine(
                        $"[bold yellow]Congratulations! Your {levelUp.SkillName} level is now {levelUp.NewLevel}![/]");
                }

                // Display slayer progress
                if (outcome.SlayerProgress is { } sp)
                {
                    AnsiConsole.WriteLine();
                    if (sp.TaskComplete)
                    {
                        AnsiConsole.MarkupLine("[bold green]Slayer task complete![/]");
                        AnsiConsole.MarkupLine("[dim]Visit a slayer master to get a new task.[/]");
                    }
                    else
                    {
                        var task = combat.Player.SlayerTask;
                        if (task != null)
                        {
                            AnsiConsole.MarkupLine(
                                $"[purple]Slayer task:[/] [dim]{sp.RemainingKills} {task.Category} remaining[/]");
                        }
                    }
                }

                // Display drops
                if (outcome.Drops.Count > 0)
                {
                    var pendingLoot = gameState.PendingLoot;
                    pendingLoot.Clear();

                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[bold]The {0} dropped:[/]", npc.Name.Value);

                    foreach (var drop in outcome.Drops)
                    {
                        var item = ItemRegistry.GetById(drop.ItemId);
                        if (item == null)
                        {
                            continue;
                        }

                        pendingLoot.Add(drop.ItemId, drop.Quantity);
                        var quantityText = drop.Quantity > 1 ? $" x{drop.Quantity}" : "";
                        AnsiConsole.MarkupLine($"  [yellow]{item.Name}{quantityText}[/]");
                    }

                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[dim]Use 'combat loot <item>' to pick up items.[/]");
                }

                break;

            case CombatOutcome.PlayerDeath:
                AnsiConsole.MarkupLine("[bold red]Oh dear, you are dead![/]");
                break;

            case CombatOutcome.PlayerFled:
                // Already handled in RunCombatLoop
                break;
        }
    }

    private IRenderable BuildCombatDisplay(CombatSession combat)
    {
        var npc = combat.Npc;
        var player = combat.Player;

        var npcHpPercent = (double)combat.NpcCurrentHitpoints / npc.Hitpoints;
        var playerHpPercent = (double)player.CurrentHealth / player.MaximumHealth;

        const int panelWidth = 40;

        var grid = new Grid();
        grid.AddColumn();

        var npcPanel = new Panel(
            new Markup($"[bold]{npc.Name}[/] [dim](level-{npc.CombatLevel})[/]\n\n" +
                       $"HP: {CreateHealthBar(npcHpPercent, npc.Hitpoints)} {combat.NpcCurrentHitpoints}/{npc.Hitpoints}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0),
            Width = panelWidth
        };
        grid.AddRow(npcPanel);

        var playerPanel = new Panel(
            new Markup(
                $"[bold green]{player.Name}[/] [dim](level-{player.CombatLevel})[/]\n\n" +
                $"HP: {CreateHealthBar(playerHpPercent, player.MaximumHealth)} {player.CurrentHealth}/{player.MaximumHealth}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0),
            Width = panelWidth
        };
        grid.AddRow(playerPanel);

        var hasPlayerAction = !string.IsNullOrWhiteSpace(_lastPlayerAction);
        var hasNpcAction = !string.IsNullOrWhiteSpace(_lastNpcAction);

        if (hasPlayerAction || hasNpcAction)
        {
            grid.AddEmptyRow();
            if (hasPlayerAction)
            {
                grid.AddRow(new Markup(_lastPlayerAction));
            }

            if (hasNpcAction)
            {
                grid.AddRow(new Markup(_lastNpcAction));
            }

            grid.AddEmptyRow();
        }

        return grid;
    }

    private static string CreateHealthBar(double percent, int maxHealth)
    {
        const int baseLength = 15;
        const double scaleFactor = 2.0;
        var barLength = baseLength + (int)(Math.Sqrt(maxHealth) * scaleFactor);
        barLength = Math.Clamp(barLength, 15, 35);

        var filled = (int)(percent * barLength);
        var empty = barLength - filled;

        return $"[green]{new string('█', filled)}[/][red]{new string('█', empty)}[/]";
    }

    private static CombatAction ShowCombatMenu()
    {
        AnsiConsole.MarkupLine("[bold]What will you do?[/]");

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("ATTACK", "INVENTORY", "MAGIC", "RUN")
                .HighlightStyle(Style.Parse("bold yellow")));

        return choice switch
        {
            "ATTACK" => CombatAction.Attack,
            "INVENTORY" => CombatAction.Inventory,
            "MAGIC" => CombatAction.Magic,
            "RUN" => CombatAction.Run,
            _ => CombatAction.Attack
        };
    }

    private static CombatStyle ChooseCombatStyle()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Choose attack style:[/]")
                .AddChoices("Accurate (Attack XP)", "Aggressive (Strength XP)",
                    "Defensive (Defence XP)", "Controlled (Shared XP)")
                .HighlightStyle(Style.Parse("bold yellow")));

        return choice switch
        {
            "Accurate (Attack XP)" => CombatStyle.Accurate,
            "Aggressive (Strength XP)" => CombatStyle.Aggressive,
            "Defensive (Defence XP)" => CombatStyle.Defensive,
            "Controlled (Shared XP)" => CombatStyle.Controlled,
            _ => CombatStyle.Accurate
        };
    }
}

internal enum CombatAction
{
    Attack,
    Inventory,
    Magic,
    Run
}