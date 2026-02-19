using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    // Состояние основной миссии кампании.
    public enum MissionState
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }

    // Описание основной миссии.
    public class CampaignMission
    {
        // Уникальный идентификатор миссии.
        public string Id { get; }

        // Отображаемое название миссии.
        public string Title { get; }

        // Краткое описание задачи.
        public string Description { get; }

        // Текущее состояние миссии.
        public MissionState State { get; private set; } = MissionState.NotStarted;

        public CampaignMission(string id, string title, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        // Запуск миссии.
        public void Start()
        {
            if (State != MissionState.NotStarted)
                throw new InvalidOperationException("Миссию можно запустить только из состояния NotStarted.");

            State = MissionState.InProgress;
        }

        // Завершение миссии с успехом.
        public void Complete()
        {
            if (State != MissionState.InProgress)
                throw new InvalidOperationException("Миссию можно завершить только из состояния InProgress.");

            State = MissionState.Completed;
        }

        // Провал миссии.
        public void Fail()
        {
            if (State != MissionState.InProgress)
                throw new InvalidOperationException("Миссию можно провалить только из состояния InProgress.");

            State = MissionState.Failed;
        }
    }

    // Модуль игрового процесса основной кампании (типовой уровень).
    public class CampaignCoreModule
    {
        // Текущая активная миссия кампании.
        public CampaignMission CurrentMission { get; private set; }

        // История завершённых миссий.
        public IReadOnlyList<CampaignMission> CompletedMissions => _completedMissions.AsReadOnly();

        private readonly List<CampaignMission> _completedMissions = new List<CampaignMission>();

        // Запуск новой миссии кампании.
        public void StartMission(CampaignMission mission)
        {
            if (mission == null)
                throw new ArgumentNullException(nameof(mission));

            if (CurrentMission != null && CurrentMission.State == MissionState.InProgress)
                throw new InvalidOperationException("Нельзя запустить новую миссию, пока текущая не завершена.");

            CurrentMission = mission;
            CurrentMission.Start();
        }

        // Отметить текущую миссию как успешно выполненную.
        public void CompleteCurrentMission()
        {
            if (CurrentMission == null)
                throw new InvalidOperationException("Текущая миссия не задана.");

            CurrentMission.Complete();
            _completedMissions.Add(CurrentMission);
            CurrentMission = null;
        }

        // Отметить текущую миссию как проваленную.
        public void FailCurrentMission()
        {
            if (CurrentMission == null)
                throw new InvalidOperationException("Текущая миссия не задана.");

            CurrentMission.Fail();
            CurrentMission = null;
        }
    }
}
