using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    /// <summary>
    /// Статус отдельной цели миссии.
    /// </summary>
    public enum ObjectiveStatus
    {
        /// <summary>
        /// Цель ещё не активирована.
        /// </summary>
        Pending,

        /// <summary>
        /// Цель активна и должна быть выполнена.
        /// </summary>
        Active,

        /// <summary>
        /// Цель успешно выполнена.
        /// </summary>
        Completed,

        /// <summary>
        /// Цель провалена.
        /// </summary>
        Failed
    }

    /// <summary>
    /// Отдельная цель внутри миссии.
    /// </summary>
    public class MissionObjective
    {
        /// <summary>
        /// Уникальный идентификатор цели.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Текстовое описание цели.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Текущий статус цели.
        /// </summary>
        public ObjectiveStatus Status { get; private set; } = ObjectiveStatus.Pending;

        /// <summary>
        /// Создаёт новую цель миссии.
        /// </summary>
        /// <param name="id">Уникальный идентификатор цели.</param>
        /// <param name="description">Описание цели.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="description"/> равны null.
        /// </exception>
        public MissionObjective(string id, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Активирует цель.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если цель уже была активирована.
        /// </exception>
        public void Activate()
        {
            if (Status != ObjectiveStatus.Pending)
                throw new InvalidOperationException("Цель можно активировать только из состояния Pending.");

            Status = ObjectiveStatus.Active;
        }

        /// <summary>
        /// Помечает цель как выполненную.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если цель не находится в состоянии Active.
        /// </exception>
        public void Complete()
        {
            if (Status != ObjectiveStatus.Active)
                throw new InvalidOperationException("Цель можно завершить только из состояния Active.");

            Status = ObjectiveStatus.Completed;
        }

        /// <summary>
        /// Помечает цель как проваленную.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если цель уже выполнена.
        /// </exception>
        public void Fail()
        {
            if (Status == ObjectiveStatus.Completed)
                throw new InvalidOperationException("Нельзя провалить уже выполненную цель.");

            Status = ObjectiveStatus.Failed;
        }
    }

    /// <summary>
    /// Миссия с набором целей.
    /// </summary>
    public class Mission
    {
        /// <summary>
        /// Уникальный идентификатор миссии.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Название миссии.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Список целей миссии.
        /// </summary>
        public IReadOnlyList<MissionObjective> Objectives => _objectives.AsReadOnly();

        private readonly List<MissionObjective> _objectives = new List<MissionObjective>();

        /// <summary>
        /// Создаёт миссию с указанным идентификатором и названием.
        /// </summary>
        /// <param name="id">Уникальный идентификатор миссии.</param>
        /// <param name="title">Название миссии.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="title"/> равны null.
        /// </exception>
        public Mission(string id, string title)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <summary>
        /// Добавляет цель в миссию.
        /// </summary>
        /// <param name="objective">Цель миссии.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="objective"/> равна null.
        /// </exception>
        public void AddObjective(MissionObjective objective)
        {
            if (objective == null)
                throw new ArgumentNullException(nameof(objective));

            _objectives.Add(objective);
        }
    }

    /// <summary>
    /// Модуль сценария, миссий и системы заданий.
    /// </summary>
    public class MissionsAndObjectivesModule
    {
        /// <summary>
        /// Список миссий кампании.
        /// </summary>
        public IReadOnlyList<Mission> Missions => _missions.AsReadOnly();

        private readonly List<Mission> _missions = new List<Mission>();

        /// <summary>
        /// Добавляет миссию в кампанию.
        /// </summary>
        /// <param name="mission">Миссия, которую необходимо добавить.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="mission"/> равна null.
        /// </exception>
        public void AddMission(Mission mission)
        {
            if (mission == null)
                throw new ArgumentNullException(nameof(mission));

            _missions.Add(mission);
        }

        /// <summary>
        /// Находит миссию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор миссии.</param>
        /// <returns>Найденная миссия.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если миссия с указанным идентификатором не найдена.
        /// </exception>
        public Mission GetMission(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор миссии не может быть пустым.", nameof(id));

            var mission = _missions.Find(m => m.Id == id);
            if (mission == null)
                throw new InvalidOperationException($"Миссия с идентификатором \"{id}\" не найдена.");

            return mission;
        }
    }
}
