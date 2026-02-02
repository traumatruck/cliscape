using CliScape.Core;
using CliScape.Core.Combat;
using CliScape.Core.Events;
using CliScape.Core.Npcs;
using CliScape.Core.Players;
using CliScape.Core.Players.Skills;

namespace CliScape.Game.Combat;

/// <summary>
///     Default implementation of <see cref="ICombatEngine" />.
/// </summary>
public sealed class CombatEngine : ICombatEngine
{
    /// <summary>
    ///     Singleton instance using default dependencies.
    /// </summary>
    public static readonly CombatEngine Instance = new(
        CombatCalculator.Instance,
        RandomProvider.Instance,
        DomainEventDispatcher.Instance);

    private readonly ICombatCalculator _calculator;
    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IRandomProvider _random;

    public CombatEngine(
        ICombatCalculator calculator,
        IRandomProvider random,
        IDomainEventDispatcher eventDispatcher)
    {
        _calculator = calculator;
        _random = random;
        _eventDispatcher = eventDispatcher;
    }

    /// <inheritdoc />
    public CombatTurnResult ExecuteTurn(CombatSession session, CombatStyle style)
    {
        // Player attacks
        var playerAttack = ExecutePlayerAttack(session, style);

        // NPC counter-attacks if combat continues
        AttackResult? npcAttack = null;
        if (!session.IsComplete)
        {
            npcAttack = ExecuteNpcAttack(session);
        }

        session.NextTurn();

        return new CombatTurnResult(playerAttack, npcAttack, session.IsComplete);
    }

    /// <inheritdoc />
    public FleeResult AttemptFlee(CombatSession session)
    {
        var success = session.AttemptFlee();
        return new FleeResult(success);
    }

    /// <inheritdoc />
    public CombatOutcomeResult ProcessOutcome(CombatSession session)
    {
        var player = session.Player;
        var npc = session.Npc;
        var rewards = session.Rewards;

        CombatOutcome outcome;
        IReadOnlyList<DroppedItem> drops = [];
        SlayerTaskProgress? slayerProgress = null;

        if (session.PlayerWon)
        {
            outcome = CombatOutcome.PlayerVictory;

            // Award slayer XP and progress task
            slayerProgress = ProcessSlayerTask(player, npc, rewards);

            // Roll drops
            drops = npc.DropTable.RollDrops();

            _eventDispatcher.Raise(new CombatEndedEvent(npc.Name, outcome));
        }
        else if (session.PlayerDied)
        {
            outcome = CombatOutcome.PlayerDeath;
            _eventDispatcher.Raise(new PlayerDiedEvent());
            _eventDispatcher.Raise(new CombatEndedEvent(npc.Name, outcome));
        }
        else
        {
            outcome = CombatOutcome.PlayerFled;
            _eventDispatcher.Raise(new CombatEndedEvent(npc.Name, outcome));
        }

        return new CombatOutcomeResult(
            outcome,
            rewards.ExperienceGained,
            rewards.LevelUps,
            drops,
            slayerProgress);
    }

    private AttackResult ExecutePlayerAttack(CombatSession session, CombatStyle style)
    {
        var player = session.Player;
        var npc = session.Npc;

        var attackRoll = _calculator.CalculatePlayerAttackRoll(player);
        var defenceRoll = _calculator.CalculateNpcDefenceRoll(npc);

        if (!_calculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            return new AttackResult(false, 0, CombatExperience.None);
        }

        var maxHit = _calculator.CalculatePlayerMaxHit(player);
        var damage = _calculator.RollDamage(maxHit);

        session.DamageNpc(damage);

        var experience = CombatExperience.None;
        if (damage > 0)
        {
            experience = _calculator.CalculateExperience(damage, style);
            AwardExperience(session, experience);
        }

        return new AttackResult(true, damage, experience);
    }

    private AttackResult ExecuteNpcAttack(CombatSession session)
    {
        var player = session.Player;
        var npc = session.Npc;
        var attack = npc.Attacks.FirstOrDefault();

        if (attack == null)
        {
            return new AttackResult(false, 0, CombatExperience.None);
        }

        var attackRoll = _calculator.CalculateNpcAttackRoll(npc);
        var defenceRoll = _calculator.CalculatePlayerDefenceRoll(player, npc);

        if (!_calculator.DoesAttackHit(attackRoll, defenceRoll))
        {
            return new AttackResult(false, 0, CombatExperience.None);
        }

        var damage = _calculator.RollDamage(attack.MaxHit);
        session.DamagePlayer(damage);

        return new AttackResult(true, damage, CombatExperience.None);
    }

    private void AwardExperience(CombatSession session, CombatExperience xp)
    {
        var player = session.Player;
        var rewards = session.Rewards;

        AwardSkillXp(player, rewards, SkillConstants.AttackSkillName, xp.AttackXp);
        AwardSkillXp(player, rewards, SkillConstants.StrengthSkillName, xp.StrengthXp);
        AwardSkillXp(player, rewards, SkillConstants.DefenceSkillName, xp.DefenceXp);
        AwardSkillXp(player, rewards, SkillConstants.HitpointsSkillName, xp.HitpointsXp);
    }

    private void AwardSkillXp(Player player, CombatRewards rewards, SkillName skillName, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        var skill = player.Skills.FirstOrDefault(s => s.Name == skillName);
        if (skill == null)
        {
            return;
        }

        var levelBefore = skill.Level.Value;
        Player.AddExperience(skill, amount);
        rewards.AddExperience(skillName.Name, amount);

        var levelAfter = skill.Level.Value;
        if (levelAfter > levelBefore)
        {
            rewards.AddLevelUp(skillName.Name, levelAfter);
            _eventDispatcher.Raise(new LevelUpEvent(skillName, levelAfter));
        }

        _eventDispatcher.Raise(new ExperienceGainedEvent(skillName, amount, skill.Level.Experience));
    }

    private SlayerTaskProgress? ProcessSlayerTask(Player player, ICombatableNpc npc, CombatRewards rewards)
    {
        var task = player.SlayerTask;

        // Check if player has an active task for this NPC's category
        if (task == null || task.IsComplete)
        {
            return null;
        }

        if (!string.Equals(task.Category, npc.SlayerCategory, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        // Award slayer XP
        var slayerXpGained = 0;
        if (npc.SlayerXp > 0)
        {
            var slayerSkill = player.Skills.FirstOrDefault(s => s.Name.Name == "Slayer");
            if (slayerSkill != null)
            {
                var levelBefore = slayerSkill.Level.Value;
                Player.AddExperience(slayerSkill, npc.SlayerXp);
                rewards.AddExperience("Slayer", npc.SlayerXp);
                slayerXpGained = npc.SlayerXp;

                var levelAfter = slayerSkill.Level.Value;
                if (levelAfter > levelBefore)
                {
                    rewards.AddLevelUp("Slayer", levelAfter);
                    _eventDispatcher.Raise(new LevelUpEvent(SkillConstants.SlayerSkillName, levelAfter));
                }
            }
        }

        // Progress the task
        var updatedTask = task.CompleteKill();
        player.SlayerTask = updatedTask;

        if (updatedTask.IsComplete)
        {
            _eventDispatcher.Raise(new SlayerTaskCompletedEvent(task.Category, task.TotalKills, task.SlayerMaster));
        }

        return new SlayerTaskProgress(updatedTask.RemainingKills, updatedTask.IsComplete, slayerXpGained);
    }
}