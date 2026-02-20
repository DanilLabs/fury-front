using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    /// <summary>
    /// Состояние основной миссии кампании.
    /// </summary>
    public enum MissionState
    {
        /// <summary>
        /// Миссия ещё не запускалась.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Миссия выполняется.
        /// </summary>
        InProgress,

        /// <summary>
        /// Миссия успешно завершена.
        /// </summary>
        Completed,

        /// <summary>
        /// Миссия провалена.
        /// </summary>
        Failed
    }

    /// <summary>
    /// Описание основной миссии кампании.
    /// </summary>
    public class CampaignMission
    {
        /// <summary>
        /// Уникальный идентификатор миссии.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Название миссии, отображаемое игроку.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Краткое текстовое описание задания.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Текущее состояние миссии.
        /// </summary>
        public MissionState State { get; private set; } = MissionState.NotStarted;

        /// <summary>
        /// Создаёт новую миссию кампании.
        /// </summary>
        /// <param name="id">Уникальный идентификатор миссии.</param>
        /// <param name="title">Название миссии.</param>
        /// <param name="description">Краткое описание цели.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если один из параметров равен null.
        /// </exception>
        public CampaignMission(string id, string title, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Запускает миссию.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если миссия уже была запущена.
        /// </exception>
        public void Start()
        {
            if (State != MissionState.NotStarted)
                throw new InvalidOperationException("Миссию можно запустить только из состояния NotStarted.");

            State = MissionState.InProgress;
        }

        /// <summary>
        /// Отмечает миссию как успешно завершённую.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если миссия не находится в состоянии InProgress.
        /// </exception>
        public void Complete()
        {
            if (State != MissionState.InProgress)
                throw new InvalidOperationException("Миссию можно завершить только из состояния InProgress.");

            State = MissionState.Completed;
        }

        /// <summary>
        /// Отмечает миссию как проваленную.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если миссия не находится в состоянии InProgress.
        /// </exception>
        public void Fail()
        {
            if (State != MissionState.InProgress)
                throw new InvalidOperationException("Миссию можно провалить только из состояния InProgress.");

            State = MissionState.Failed;
        }
    }

    /// <summary>
    /// Модуль игрового процесса основной кампании (типовой уровень).
    /// </summary>
    public class CampaignCoreModule
    {
        /// <summary>
        /// Текущая активная миссия кампании.
        /// </summary>
        public CampaignMission CurrentMission { get; private set; }

        /// <summary>
        /// Список завершённых миссий кампании.
        /// </summary>
        public IReadOnlyList<CampaignMission> CompletedMissions => _completedMissions.AsReadOnly();

        private readonly List<CampaignMission> _completedMissions = new List<CampaignMission>();

        /// <summary>
        /// Запускает новую миссию кампании.
        /// </summary>
        /// <param name="mission">Миссия, которую необходимо запустить.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="mission"/> равен null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если другая миссия уже выполняется.
        /// </exception>
        public void StartMission(CampaignMission mission)
        {
            if (mission == null)
                throw new ArgumentNullException(nameof(mission));

            if (CurrentMission != null && CurrentMission.State == MissionState.InProgress)
                throw new InvalidOperationException("Нельзя запустить новую миссию, пока текущая не завершена.");

            CurrentMission = mission;
            CurrentMission.Start();
        }

        /// <summary>
        /// Отмечает текущую миссию как успешно выполненную и добавляет её в историю.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если текущая миссия не задана.
        /// </exception>
        public void CompleteCurrentMission()
        {
            if (CurrentMission == null)
                throw new InvalidOperationException("Текущая миссия не задана.");

            CurrentMission.Complete();
            _completedMissions.Add(CurrentMission);
            CurrentMission = null;
        }

        /// <summary>
        /// Отмечает текущую миссию как проваленную.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если текущая миссия не задана.
        /// </exception>
        public void FailCurrentMission()
        {
            if (CurrentMission == null)
                throw new InvalidOperationException("Текущая миссия не задана.");

            CurrentMission.Fail();
            CurrentMission = null;
        }
    }
}
