using CliScape.Content.Items;
using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
///     Initiates combat with an NPC.
/// </summary>
public class CombatAttackCommand : Command<CombatAttackCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;
    private string _lastNpcAction = "";
    private string _lastPlayerAction = "";

    public static string CommandName => "attack";

    public override int Execute(CommandContext context, CombatAttackCommandSettings settings,
        CancellationToken cancellationToken)
    {
        var player = _gameState.GetPlayer();
        var location = player.CurrentLocation;

        // Check if already in combat
        if (_gameState.IsInCombat)
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
        var combat = _gameState.StartCombat(npc);

        // Run the combat loop
        RunCombatLoop(combat, player);

        // Handle combat outcome
        HandleCombatOutcome(combat, npc);

        _gameState.EndCombat();

        return (int)ExitCode.Success;
    }

    private void RunCombatLoop(CombatSession combat, Player player)
    {
        var combatStyle = CombatStyle.Accurate; // Default style

        while (!combat.IsComplete)
        {
            // Clear screen and display current combat status
            AnsiConsole.Clear();
            AnsiConsole.Write(BuildCombatDisplay(combat));

            // Show menu and get player action
            var action = ShowCombatMenu();

            switch (action)
            {
                case CombatAction.Attack:
                    combatStyle = ChooseCombatStyle();
                    ExecutePlayerAttack(combat, combatStyle);
                    break;

                case CombatAction.Inventory:
                    _lastPlayerAction = "[yellow]Your inventory is empty.[/]";
                    _lastNpcAction = "";
                    continue; // Don't advance turn for bag viewing

                case CombatAction.Magic:
                    _lastPlayerAction = "[blue]You don't know any spells yet.[/]";
                    _lastNpcAction = "";
                    continue; // Magic not implemented yet

                case CombatAction.Run:
                    if (AttemptFlee(combat))
                    {
                        AnsiConsole.Clear();
                        AnsiConsole.Write(BuildCombatDisplay(combat));
                        return;
                    }

                    break;
            }

            // NPC attacks if combat continues
            if (!combat.IsComplete)
            {
                ExecuteNpcAttack(combat);
            }

            combat.NextTurn();
        }

        // Show final combat state
        AnsiConsole.Clear();
        AnsiConsole.Write(BuildCombatDisplay(combat));
    }

    private IRenderable BuildCombatDisplay(CombatSession combat)
    {
        var npc = combat.Npc;
        var player = combat.Player;

        var npcHpPercent = (double)combat.NpcCurrentHitpoints / npc.Hitpoints;
        var playerHpPercent = (double)player.CurrentHealth / player.MaximumHealth;

        const int panelWidth = 40;

        // Build the combat display as a single renderable
        var grid = new Grid();
        grid.AddColumn();

        // NPC status panel
        var npcPanel = new Panel(
            new Markup($"[bold]{npc.Name}[/] [dim](level-{npc.CombatLevel})[/]\n\n" +
                       $"HP: {CreateHealthBar(npcHpPercent, npc.Hitpoints)} {combat.NpcCurrentHitpoints}/{npc.Hitpoints}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0),
            Width = panelWidth
        };
        grid.AddRow(npcPanel);

        // Player status panel
        var playerPanel = new Panel(
            new Markup($"[bold green]{player.Name}[/] [dim](level-{player.CombatLevel})[/]\n\n" +
                       $"HP: {CreateHealthBar(playerHpPercent, player.MaximumHealth)} {player.CurrentHealth}/{player.MaximumHealth}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0),
            Width = panelWidth
        };
        grid.AddRow(playerPanel);

        // Combat action messages - only add if there's something to show
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
        // Dynamic bar length based on max health with subtle curve
        // Uses square root to give small increases (e.g., 3 HP → 18 chars, 10 HP → 21 chars, 50 HP → 29 chars)
        const int baseLength = 15;
        const double scaleFactor = 2.0;
        var barLength = baseLength + (int)(Math.Sqrt(maxHealth) * scaleFactor);
        barLength = Math.Clamp(barLength, 15, 35); // Keep reasonable bounds
        
        var filled = (int)(percent * barLength);
        var empty = barLength - filled;

        // OSRS style: green for remaining HP, red for missing HP
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

    private void ExecutePlayerAttack(CombatSession combat, CombatStyle style)
    {
        var player = combat.Player;
        var npc = combat.Npc;

        var attackRoll = CombatCalculator.Instance.CalculatePlayerAttackRoll(player);
        var defenceRoll = CombatCalculator.Instance.CalculateNpcDefenceRoll(npc);

        if (CombatCalculator.Instance.DoesAttackHit(attackRoll, defenceRoll))
        {
            var maxHit = CombatCalculator.Instance.CalculatePlayerMaxHit(player);
            var damage = CombatCalculator.Instance.RollDamage(maxHit);

            combat.DamageNpc(damage);

            if (damage > 0)
            {
                _lastPlayerAction = $"[green]You hit the {npc.Name} for {damage} damage![/]";

                // Award experience
                var xp = CombatCalculator.Instance.CalculateExperience(damage, style);
                AwardExperience(combat, xp);
            }
            else
            {
                _lastPlayerAction = $"[dim]You hit the {npc.Name} but deal no damage.[/]";
            }
        }
        else
        {
            _lastPlayerAction = $"[dim]Your attack misses the {npc.Name}.[/]";
        }
    }

    private static void AwardExperience(CombatSession combat, CombatExperience xp)
    {
        var player = combat.Player;
        var rewards = combat.Rewards;

        AwardSkillXp(player, rewards, "Attack", xp.AttackXp);
        AwardSkillXp(player, rewards, "Strength", xp.StrengthXp);
        AwardSkillXp(player, rewards, "Defence", xp.DefenceXp);
        AwardSkillXp(player, rewards, "Hitpoints", xp.HitpointsXp);
    }

    private static void AwardSkillXp(Player player, CombatRewards rewards, string skillName, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        var skill = player.Skills.First(s => s.Name.Name == skillName);
        var levelBefore = skill.Level.Value;

        Player.AddExperience(skill, amount);
        rewards.AddExperience(skillName, amount);

        var levelAfter = skill.Level.Value;
        if (levelAfter > levelBefore)
        {
            rewards.AddLevelUp(skillName, levelAfter);
        }
    }

    private void ExecuteNpcAttack(CombatSession combat)
    {
        var player = combat.Player;
        var npc = combat.Npc;
        var attack = npc.Attacks.FirstOrDefault();

        if (attack == null)
        {
            _lastNpcAction = "";
            return;
        }

        var attackRoll = CombatCalculator.Instance.CalculateNpcAttackRoll(npc);
        var defenceRoll = CombatCalculator.Instance.CalculatePlayerDefenceRoll(player, npc);

        if (CombatCalculator.Instance.DoesAttackHit(attackRoll, defenceRoll))
        {
            var damage = CombatCalculator.Instance.RollDamage(attack.MaxHit);

            combat.DamagePlayer(damage);

            if (damage > 0)
            {
                _lastNpcAction = $"[red]The {npc.Name} hits you for {damage} damage![/]";
            }
            else
            {
                _lastNpcAction = $"[dim]The {npc.Name} hits you but deals no damage.[/]";
            }
        }
        else
        {
            _lastNpcAction = $"[dim]The {npc.Name}'s attack misses you.[/]";
        }
    }

    private bool AttemptFlee(CombatSession combat)
    {
        if (combat.AttemptFlee())
        {
            _lastPlayerAction = "[green]Got away safely![/]";
            _lastNpcAction = "";
            return true;
        }

        _lastPlayerAction = "[red]Can't escape![/]";
        _lastNpcAction = "";
        return false;
    }

    private void HandleCombatOutcome(CombatSession combat, ICombatableNpc npc)
    {
        if (combat.PlayerWon)
        {
            AnsiConsole.MarkupLine($"[bold green]You defeated the {npc.Name}![/]");

            // Display XP summary
            DisplayXpSummary(combat);

            // Award slayer XP if applicable and player has an active slayer task
            AwardSlayerXp(combat, npc);

            // Progress slayer task
            ProgressSlayerTask(combat, npc);

            // Handle drops
            HandleDrops(combat, npc);

            // Display level-ups
            DisplayLevelUps(combat);
        }
        else if (combat.PlayerDied)
        {
            AnsiConsole.MarkupLine("[bold red]Oh dear, you are dead![/]");
        }
        else if (combat.PlayerFled)
        {
            // Already handled in AttemptFlee
        }
    }

    private static void ProgressSlayerTask(CombatSession combat, ICombatableNpc npc)
    {
        var player = combat.Player;
        var task = player.SlayerTask;

        // Check if player has an active task for this NPC's category
        if (task == null || task.IsComplete)
        {
            return;
        }

        if (!string.Equals(task.Category, npc.SlayerCategory, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Progress the task
        player.SlayerTask = task.CompleteKill();

        if (player.SlayerTask.IsComplete)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold green]Slayer task complete![/]");
            AnsiConsole.MarkupLine("[dim]Visit a slayer master to get a new task.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[purple]Slayer task:[/] [dim]{player.SlayerTask.RemainingKills} {task.Category} remaining[/]");
        }
    }

    private static void DisplayXpSummary(CombatSession combat)
    {
        var rewards = combat.Rewards;
        if (rewards.ExperienceGained.Count == 0)
        {
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]Experience gained:[/]");
        foreach (var (skillName, xp) in rewards.ExperienceGained)
        {
            AnsiConsole.MarkupLine($"  [cyan]{skillName}[/]: [white]+{xp} XP[/]");
        }
    }

    private static void AwardSlayerXp(CombatSession combat, ICombatableNpc npc)
    {
        // Only award slayer XP if player has an active task for this NPC's category
        var player = combat.Player;
        var task = player.SlayerTask;

        if (task == null || task.IsComplete)
        {
            return;
        }

        // Check if this NPC matches the slayer task category
        if (!string.Equals(task.Category, npc.SlayerCategory, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (npc.SlayerXp <= 0)
        {
            return;
        }

        var slayerSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Slayer");

        if (slayerSkill == null)
        {
            return;
        }

        var levelBefore = slayerSkill.Level.Value;
        Player.AddExperience(slayerSkill, npc.SlayerXp);
        combat.Rewards.AddExperience("Slayer", npc.SlayerXp);

        var levelAfter = slayerSkill.Level.Value;
        if (levelAfter > levelBefore)
        {
            combat.Rewards.AddLevelUp("Slayer", levelAfter);
        }
    }

    private void HandleDrops(CombatSession combat, ICombatableNpc npc)
    {
        var drops = npc.DropTable.RollDrops();

        if (drops.Count == 0)
        {
            return;
        }

        // Clear any previous pending loot and add new drops
        var pendingLoot = _gameState.PendingLoot;
        pendingLoot.Clear();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold]The {0} dropped:[/]", npc.Name.Value);

        foreach (var drop in drops)
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

    private static void DisplayLevelUps(CombatSession combat)
    {
        var levelUps = combat.Rewards.LevelUps;
        if (levelUps.Count == 0)
        {
            return;
        }

        AnsiConsole.WriteLine();
        foreach (var levelUp in levelUps)
        {
            AnsiConsole.MarkupLine($"[bold yellow]Congratulations! Your {levelUp.SkillName} level is now {levelUp.NewLevel}![/]");
        }
    }
}

internal enum CombatAction
{
    Attack,
    Inventory,
    Magic,
    Run
}