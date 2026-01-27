using CliScape.Core.Npcs;
using CliScape.Core.Players;

namespace CliScape.Core.Combat;

/// <summary>
/// Represents an active combat session between a player and an NPC.
/// Tracks the current state of combat including HP, turns, and outcomes.
/// </summary>
public class CombatSession
{
    public CombatSession(Player player, ICombatableNpc npc)
    {
        Player = player;
        Npc = npc;
        NpcCurrentHitpoints = npc.Hitpoints;
        TurnCount = 0;
        IsComplete = false;
        PlayerFled = false;
    }

    public Player Player { get; }
    public ICombatableNpc Npc { get; }
    public int NpcCurrentHitpoints { get; private set; }
    public int TurnCount { get; private set; }
    public bool IsComplete { get; private set; }
    public bool PlayerFled { get; private set; }
    
    /// <summary>
    /// Whether the player won (NPC died)
    /// </summary>
    public bool PlayerWon => IsComplete && NpcCurrentHitpoints <= 0 && Player.CurrentHealth > 0;
    
    /// <summary>
    /// Whether the player died
    /// </summary>
    public bool PlayerDied => IsComplete && Player.CurrentHealth <= 0;

    /// <summary>
    /// Apply damage to the NPC
    /// </summary>
    public void DamageNpc(int damage)
    {
        NpcCurrentHitpoints = Math.Max(0, NpcCurrentHitpoints - damage);
        
        if (NpcCurrentHitpoints <= 0)
        {
            IsComplete = true;
        }
    }

    /// <summary>
    /// Apply damage to the player
    /// </summary>
    public void DamagePlayer(int damage)
    {
        Player.TakeDamage(damage);
        
        if (Player.CurrentHealth <= 0)
        {
            IsComplete = true;
        }
    }

    /// <summary>
    /// Increment the turn counter
    /// </summary>
    public void NextTurn()
    {
        TurnCount++;
    }

    /// <summary>
    /// Attempt to flee from combat
    /// </summary>
    /// <returns>True if the flee attempt was successful</returns>
    public bool AttemptFlee()
    {
        // Simple flee mechanic: higher chance to flee on later turns
        // Base 25% chance + 5% per turn, max 75%
        var fleeChance = Math.Min(75, 25 + (TurnCount * 5));
        var roll = Random.Shared.Next(100);
        
        if (roll < fleeChance)
        {
            PlayerFled = true;
            IsComplete = true;
            return true;
        }
        
        return false;
    }
}
