using CliScape.Core.Combat;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Game;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliScape.Cli.Commands.Combat;

/// <summary>
/// Initiates combat with an NPC and runs the Pokemon-style combat menu loop.
/// </summary>
public class CombatAttackCommand : Command<CombatAttackCommandSettings>, ICommand
{
    private readonly GameState _gameState = GameState.Instance;
    
    public static string CommandName => "attack";

    public override int Execute(CommandContext context, CombatAttackCommandSettings settings, CancellationToken cancellationToken)
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
            AnsiConsole.MarkupLine("[dim]Use 'combat list' to see available NPCs.[/]");
            return (int)ExitCode.BadRequest;
        }
        
        // Start combat
        var combat = _gameState.StartCombat(npc);
        
        AnsiConsole.MarkupLine($"[bold red]A wild {npc.Name} appears![/]");
        AnsiConsole.WriteLine();
        
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
            // Display combat status
            DisplayCombatStatus(combat);
            
            // Show menu and get player action
            var action = ShowCombatMenu();
            
            switch (action)
            {
                case CombatAction.Fight:
                    combatStyle = ChooseCombatStyle();
                    ExecutePlayerAttack(combat, combatStyle);
                    break;
                    
                case CombatAction.Bag:
                    ShowBagMenu();
                    continue; // Don't advance turn for bag viewing
                    
                case CombatAction.Magic:
                    ShowMagicMenu();
                    continue; // Magic not implemented yet
                    
                case CombatAction.Run:
                    if (AttemptFlee(combat))
                    {
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
    }
    
    private void DisplayCombatStatus(CombatSession combat)
    {
        var npc = combat.Npc;
        var player = combat.Player;
        
        var npcHpPercent = (double)combat.NpcCurrentHitpoints / npc.Hitpoints;
        var playerHpPercent = (double)player.CurrentHealth / player.MaximumHealth;
        
        AnsiConsole.WriteLine();
        
        // NPC status (top-left style like Pokemon)
        var npcPanel = new Panel(
            new Markup($"[bold]{npc.Name}[/] Lv{npc.CombatLevel}\n" +
                      $"HP: {CreateHealthBar(npcHpPercent)} {combat.NpcCurrentHitpoints}/{npc.Hitpoints}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(npcPanel);
        
        // Player status (bottom-right style)
        var playerPanel = new Panel(
            new Markup($"[bold green]{player.Name}[/] Lv{player.CombatLevel}\n" +
                      $"HP: {CreateHealthBar(playerHpPercent)} {player.CurrentHealth}/{player.MaximumHealth}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(playerPanel);
        
        AnsiConsole.WriteLine();
    }
    
    private static string CreateHealthBar(double percent)
    {
        const int barLength = 20;
        var filled = (int)(percent * barLength);
        var empty = barLength - filled;
        
        var color = percent switch
        {
            > 0.5 => "green",
            > 0.2 => "yellow",
            _ => "red"
        };
        
        return $"[{color}]{new string('█', filled)}[/][dim]{new string('░', empty)}[/]";
    }
    
    private static CombatAction ShowCombatMenu()
    {
        AnsiConsole.MarkupLine("[bold]What will you do?[/]");
        
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("FIGHT", "BAG", "MAGIC", "RUN")
                .HighlightStyle(Style.Parse("bold yellow")));
        
        return choice switch
        {
            "FIGHT" => CombatAction.Fight,
            "BAG" => CombatAction.Bag,
            "MAGIC" => CombatAction.Magic,
            "RUN" => CombatAction.Run,
            _ => CombatAction.Fight
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
    
    private static void ExecutePlayerAttack(CombatSession combat, CombatStyle style)
    {
        var player = combat.Player;
        var npc = combat.Npc;
        
        var attackRoll = CombatCalculator.CalculatePlayerAttackRoll(player);
        var defenceRoll = CombatCalculator.CalculateNpcDefenceRoll(npc);
        
        if (CombatCalculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            var maxHit = CombatCalculator.CalculatePlayerMaxHit(player);
            var damage = CombatCalculator.RollDamage(maxHit);
            
            combat.DamageNpc(damage);
            
            if (damage > 0)
            {
                AnsiConsole.MarkupLine($"[green]You hit the {npc.Name} for {damage} damage![/]");
                
                // Award experience
                var xp = CombatCalculator.CalculateExperience(damage, style);
                AwardExperience(player, xp, style);
            }
            else
            {
                AnsiConsole.MarkupLine($"[dim]You hit the {npc.Name} but deal no damage.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine($"[dim]Your attack misses the {npc.Name}.[/]");
        }
    }
    
    private static void AwardExperience(Player player, CombatExperience xp, CombatStyle style)
    {
        if (xp.AttackXp > 0)
        {
            var skill = player.Skills.First(s => s.Name.Name == "Attack");
            Player.AddExperience(skill, xp.AttackXp);
        }
        if (xp.StrengthXp > 0)
        {
            var skill = player.Skills.First(s => s.Name.Name == "Strength");
            Player.AddExperience(skill, xp.StrengthXp);
        }
        if (xp.DefenceXp > 0)
        {
            var skill = player.Skills.First(s => s.Name.Name == "Defence");
            Player.AddExperience(skill, xp.DefenceXp);
        }
        if (xp.HitpointsXp > 0)
        {
            var skill = player.Skills.First(s => s.Name.Name == "Hitpoints");
            Player.AddExperience(skill, xp.HitpointsXp);
        }
    }
    
    private static void ExecuteNpcAttack(CombatSession combat)
    {
        var player = combat.Player;
        var npc = combat.Npc;
        var attack = npc.Attacks.FirstOrDefault();
        
        if (attack == null) return;
        
        var attackRoll = CombatCalculator.CalculateNpcAttackRoll(npc);
        var defenceRoll = CombatCalculator.CalculatePlayerDefenceRoll(player);
        
        if (CombatCalculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            var damage = CombatCalculator.RollDamage(attack.MaxHit);
            
            combat.DamagePlayer(damage);
            
            if (damage > 0)
            {
                AnsiConsole.MarkupLine($"[red]The {npc.Name} hits you for {damage} damage![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[dim]The {npc.Name} hits you but deals no damage.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine($"[dim]The {npc.Name}'s attack misses you.[/]");
        }
    }
    
    private static void ShowBagMenu()
    {
        AnsiConsole.MarkupLine("[yellow]Your bag is empty.[/]");
        AnsiConsole.MarkupLine("[dim]Inventory system coming soon![/]");
        AnsiConsole.WriteLine();
    }
    
    private static void ShowMagicMenu()
    {
        AnsiConsole.MarkupLine("[blue]You don't know any spells yet.[/]");
        AnsiConsole.MarkupLine("[dim]Magic system coming soon![/]");
        AnsiConsole.WriteLine();
    }
    
    private static bool AttemptFlee(CombatSession combat)
    {
        AnsiConsole.MarkupLine("[yellow]You attempt to flee...[/]");
        
        if (combat.AttemptFlee())
        {
            AnsiConsole.MarkupLine("[green]Got away safely![/]");
            return true;
        }
        
        AnsiConsole.MarkupLine("[red]Can't escape![/]");
        return false;
    }
    
    private void HandleCombatOutcome(CombatSession combat, ICombatableNpc npc)
    {
        AnsiConsole.WriteLine();
        
        if (combat.PlayerWon)
        {
            AnsiConsole.MarkupLine($"[bold green]You defeated the {npc.Name}![/]");
            
            // Award slayer XP if applicable
            if (npc.SlayerXp > 0)
            {
                var slayerSkill = combat.Player.Skills.FirstOrDefault(s => s.Name.Name == "Slayer");
                if (slayerSkill != null)
                {
                    Player.AddExperience(slayerSkill, npc.SlayerXp);
                    AnsiConsole.MarkupLine($"[purple]You gain {npc.SlayerXp} Slayer experience.[/]");
                }
            }
        }
        else if (combat.PlayerDied)
        {
            AnsiConsole.MarkupLine($"[bold red]Oh dear, you are dead![/]");
        }
        else if (combat.PlayerFled)
        {
            // Already handled in AttemptFlee
        }
    }
}

internal enum CombatAction
{
    Fight,
    Bag,
    Magic,
    Run
}