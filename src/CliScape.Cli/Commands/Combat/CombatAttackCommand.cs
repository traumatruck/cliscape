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
    
    private string _lastPlayerAction = "";
    private string _lastNpcAction = "";
    private int _displayStartRow = -1;

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
        _displayStartRow = Console.CursorTop;
        
        while (!combat.IsComplete)
        {
            // Move cursor back to starting position and clear from there
            ClearCombatDisplay();
            
            DisplayCombatStatus(combat);
            
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
        
        // Show final combat state (overwrite previous)
        ClearCombatDisplay();
        DisplayCombatStatus(combat);
    }
    
    private void ClearCombatDisplay()
    {
        var currentRow = Console.CursorTop;
        if (currentRow > _displayStartRow)
        {
            // Move to start row and clear everything from there
            Console.SetCursorPosition(0, _displayStartRow);
            for (var i = _displayStartRow; i < currentRow; i++)
            {
                AnsiConsole.Write("\u001b[2K\n"); // Clear line and move down
            }
            Console.SetCursorPosition(0, _displayStartRow);
        }
    }
    
    private void DisplayCombatStatus(CombatSession combat)
    {
        var npc = combat.Npc;
        var player = combat.Player;
        
        var npcHpPercent = (double)combat.NpcCurrentHitpoints / npc.Hitpoints;
        var playerHpPercent = (double)player.CurrentHealth / player.MaximumHealth;
        
        // NPC status (top-left style like Pokemon)
        var npcPanel = new Panel(
            new Markup($"[bold]{npc.Name}[/] [dim](level-{npc.CombatLevel})[/]\n" +
                      $"HP: {CreateHealthBar(npcHpPercent)} {combat.NpcCurrentHitpoints}/{npc.Hitpoints}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(npcPanel);
        
        // Player status (bottom-right style)
        var playerPanel = new Panel(
            new Markup($"[bold green]{player.Name}[/] [dim](level-{player.CombatLevel})[/]\n" +
                      $"HP: {CreateHealthBar(playerHpPercent)} {player.CurrentHealth}/{player.MaximumHealth}"))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(playerPanel);
        
        AnsiConsole.WriteLine();
        
        // Display last combat actions (always output 2 lines for consistent spacing)
        var playerActionText = string.IsNullOrEmpty(_lastPlayerAction) ? " " : _lastPlayerAction;
        var npcActionText = string.IsNullOrEmpty(_lastNpcAction) ? " " : _lastNpcAction;
        
        AnsiConsole.MarkupLine(playerActionText);
        AnsiConsole.MarkupLine(npcActionText);
        
        AnsiConsole.WriteLine();
    }
    
    private static string CreateHealthBar(double percent)
    {
        const int barLength = 20;
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
        
        var attackRoll = CombatCalculator.CalculatePlayerAttackRoll(player);
        var defenceRoll = CombatCalculator.CalculateNpcDefenceRoll(npc);
        
        if (CombatCalculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            var maxHit = CombatCalculator.CalculatePlayerMaxHit(player);
            var damage = CombatCalculator.RollDamage(maxHit);
            
            combat.DamageNpc(damage);
            
            if (damage > 0)
            {
                _lastPlayerAction = $"[green]You hit the {npc.Name} for {damage} damage![/]";
                
                // Award experience
                var xp = CombatCalculator.CalculateExperience(damage, style);
                AwardExperience(player, xp, style);
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
        
        var attackRoll = CombatCalculator.CalculateNpcAttackRoll(npc);
        var defenceRoll = CombatCalculator.CalculatePlayerDefenceRoll(player, npc);
        
        if (CombatCalculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            var damage = CombatCalculator.RollDamage(attack.MaxHit);
            
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
    Attack,
    Inventory,
    Magic,
    Run
}