using System;
using System.Collections.Generic;

namespace FuryFront.Core.Combat
{
    // Поведение бота в бою.
    public enum AiBehavior
    {
        Passive,
        Defensive,
        Aggressive
    }

    // Текущая тактическая задача бота.
    public enum AiTask
    {
        Idle,
        Patrol,
        AttackPlayer,
        TakeCover,
        Retreat
    }

    // Параметры ИИ бойца.
    public class CombatAiAgent
    {
        public string Id { get; }
        public string DisplayName { get; }
        public AiBehavior Behavior { get; private set; }
        public AiTask CurrentTask { get; private set; }

        // Дистанция до игрока в метрах.
        public float DistanceToPlayer { get; set; }

        // Уровень угрозы игрока по оценке ИИ.
        public float ThreatLevel { get; set; }

        public CombatAiAgent(string id, string displayName, AiBehavior behavior)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Behavior = behavior;
            CurrentTask = AiTask.Idle;
        }

        // Обновить задачи бота на основе текущих параметров.
        public void UpdateDecision()
        {
            if (ThreatLevel <= 0.1f)
            {
                CurrentTask = AiTask.Patrol;
                return;
            }

            switch (Behavior)
            {
                case AiBehavior.Passive:
                    DecidePassive();
                    break;
                case AiBehavior.Defensive:
                    DecideDefensive();
                    break;
                case AiBehavior.Aggressive:
                    DecideAggressive();
                    break;
            }
        }

        private void DecidePassive()
        {
            if (DistanceToPlayer < 5.0f)
            {
                CurrentTask = AiTask.Retreat;
            }
            else
            {
                CurrentTask = AiTask.Idle;
            }
        }

        private void DecideDefensive()
        {
            if (DistanceToPlayer < 10.0f && ThreatLevel > 0.5f)
            {
                CurrentTask = AiTask.TakeCover;
            }
            else if (ThreatLevel > 0.7f)
            {
                CurrentTask = AiTask.Retreat;
            }
            else
            {
                CurrentTask = AiTask.Patrol;
            }
        }

        private void DecideAggressive()
        {
            if (DistanceToPlayer < 20.0f)
            {
                CurrentTask = AiTask.AttackPlayer;
            }
            else
            {
                CurrentTask = AiTask.Patrol;
            }
        }
    }

    // Модуль искусственного интеллекта противников и союзников.
    public class CombatAIModule
    {
        // Зарегистрированные агенты ИИ.
        public IReadOnlyList<CombatAiAgent> Agents => _agents.AsReadOnly();

        private readonly List<CombatAiAgent> _agents = new List<CombatAiAgent>();

        // Добавить нового ИИ-агента в систему.
        public void RegisterAgent(CombatAiAgent agent)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));

            _agents.Add(agent);
        }

        // Выполнить обновление решений для всех агентов.
        public void UpdateAllAgents()
        {
            foreach (var agent in _agents)
            {
                agent.UpdateDecision();
            }
        }
    }
}
