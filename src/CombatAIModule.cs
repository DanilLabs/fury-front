using System;
using System.Collections.Generic;

namespace FuryFront.Core.Combat
{
    /// <summary>
    /// Поведение бота в бою.
    /// </summary>
    public enum AiBehavior
    {
        /// <summary>
        /// Пассивное поведение, бот избегает боя.
        /// </summary>
        Passive,

        /// <summary>
        /// Оборонительное поведение, бот избегает лишнего риска.
        /// </summary>
        Defensive,

        /// <summary>
        /// Агрессивное поведение, бот активно вступает в бой.
        /// </summary>
        Aggressive
    }

    /// <summary>
    /// Текущая тактическая задача бота.
    /// </summary>
    public enum AiTask
    {
        /// <summary>
        /// Бездействие.
        /// </summary>
        Idle,

        /// <summary>
        /// Патрулирование территории.
        /// </summary>
        Patrol,

        /// <summary>
        /// Атака игрока.
        /// </summary>
        AttackPlayer,

        /// <summary>
        /// Поиск укрытия.
        /// </summary>
        TakeCover,

        /// <summary>
        /// Отступление.
        /// </summary>
        Retreat
    }

    /// <summary>
    /// Параметры и текущее состояние ИИ‑бойца.
    /// </summary>
    public class CombatAiAgent
    {
        /// <summary>
        /// Уникальный идентификатор агента.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Отображаемое имя агента.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Базовое поведение ИИ.
        /// </summary>
        public AiBehavior Behavior { get; private set; }

        /// <summary>
        /// Текущая тактическая задача.
        /// </summary>
        public AiTask CurrentTask { get; private set; }

        /// <summary>
        /// Дистанция до игрока в метрах.
        /// </summary>
        public float DistanceToPlayer { get; set; }

        /// <summary>
        /// Оценка уровня угрозы игрока.
        /// </summary>
        public float ThreatLevel { get; set; }

        /// <summary>
        /// Создаёт нового ИИ‑агента с указанными параметрами.
        /// </summary>
        /// <param name="id">Уникальный идентификатор агента.</param>
        /// <param name="displayName">Отображаемое имя агента.</param>
        /// <param name="behavior">Базовое поведение ИИ.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="displayName"/> равны null.
        /// </exception>
        public CombatAiAgent(string id, string displayName, AiBehavior behavior)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Behavior = behavior;
            CurrentTask = AiTask.Idle;
        }

        /// <summary>
        /// Обновляет текущую задачу бота на основе дистанции и уровня угрозы.
        /// </summary>
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

        /// <summary>
        /// Логика выбора задачи для пассивного поведения.
        /// </summary>
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

        /// <summary>
        /// Логика выбора задачи для оборонительного поведения.
        /// </summary>
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

        /// <summary>
        /// Логика выбора задачи для агрессивного поведения.
        /// </summary>
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

    /// <summary>
    /// Модуль искусственного интеллекта противников и союзников.
    /// </summary>
    public class CombatAIModule
    {
        /// <summary>
        /// Зарегистрированные агенты ИИ.
        /// </summary>
        public IReadOnlyList<CombatAiAgent> Agents => _agents.AsReadOnly();

        private readonly List<CombatAiAgent> _agents = new List<CombatAiAgent>();

        /// <summary>
        /// Регистрирует нового ИИ‑агента в системе.
        /// </summary>
        /// <param name="agent">Агент, которого необходимо добавить.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="agent"/> равен null.
        /// </exception>
        public void RegisterAgent(CombatAiAgent agent)
        {
            if (agent == null)
                throw new ArgumentNullException(nameof(agent));

            _agents.Add(agent);
        }

        /// <summary>
        /// Обновляет решения для всех зарегистрированных ИИ‑агентов.
        /// </summary>
        public void UpdateAllAgents()
        {
            foreach (var agent in _agents)
            {
                agent.UpdateDecision();
            }
        }
    }
}
