using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    // Статус отдельной цели миссии.
    public enum ObjectiveStatus
    {
        Pending,
        Active,
        Completed,
        Failed
    }

    // Отдельная цель внутри миссии.
    public class MissionObjective
    {
        public string Id { get; }
        public string Description { get; }
        public ObjectiveStatus Status { get; private set; } = ObjectiveStatus.Pending;

        public MissionObjective(string id, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void Activate()
        {
            if (Status != ObjectiveStatus.Pending)
                throw new InvalidOperationException("Цель можно активировать только из состояния Pending.");

            Status = ObjectiveStatus.Active;
        }

        public void Complete()
        {
            if (Status != ObjectiveStatus.Active)
                throw new InvalidOperationException("Цель можно завершить только из состояния Active.");

            Status = ObjectiveStatus.Completed;
        }

        public void Fail()
        {
            if (Status == ObjectiveStatus.Completed)
                throw new InvalidOperationException("Нельзя провалить уже выполненную цель.");

            Status = ObjectiveStatus.Failed;
        }
    }

    // Миссия с набором целей.
    public class Mission
    {
        public string Id { get; }
        public string Title { get; }
        public IReadOnlyList<MissionObjective> Objectives => _objectives.AsReadOnly();

        private readonly List<MissionObjective> _objectives = new List<MissionObjective>();

        public Mission(string id, string title)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public void AddObjective(MissionObjective objective)
        {
            if (objective == null)
                throw new ArgumentNullException(nameof(objective));

            _objectives.Add(objective);
        }
    }

    // Модуль сценария, миссий и системы заданий.
    public class MissionsAndObjectivesModule
    {
        public IReadOnlyList<Mission> Missions => _missions.AsReadOnly();

        private readonly List<Mission> _missions = new List<Mission>();

        // Добавить миссию в кампанию.
        public void AddMission(Mission mission)
        {
            if (mission == null)
                throw new ArgumentNullException(nameof(mission));

            _missions.Add(mission);
        }

        // Найти миссию по идентификатору.
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
