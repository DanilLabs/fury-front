using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    /// <summary>
    /// Снимок сохранения кампании.
    /// </summary>
    public class CampaignSaveSnapshot
    {
        /// <summary>
        /// Уникальный идентификатор сохранения.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Название сохранения, отображаемое игроку.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Идентификатор текущей миссии.
        /// </summary>
        public string CurrentMissionId { get; }

        /// <summary>
        /// Время создания сохранения (UTC).
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Прогресс кампании в процентах (0..100).
        /// </summary>
        public int ProgressPercent { get; }

        /// <summary>
        /// Создаёт снимок сохранения кампании.
        /// </summary>
        /// <param name="id">Уникальный идентификатор сохранения.</param>
        /// <param name="title">Название сохранения.</param>
        /// <param name="currentMissionId">Идентификатор текущей миссии.</param>
        /// <param name="createdAt">Момент создания сохранения.</param>
        /// <param name="progressPercent">Прогресс кампании в процентах.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если один из строковых параметров равен null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если прогресс вне диапазона 0..100.
        /// </exception>
        public CampaignSaveSnapshot(
            string id,
            string title,
            string currentMissionId,
            DateTime createdAt,
            int progressPercent)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            CurrentMissionId = currentMissionId ?? throw new ArgumentNullException(nameof(currentMissionId));
            CreatedAt = createdAt;

            if (progressPercent < 0 || progressPercent > 100)
                throw new ArgumentOutOfRangeException(nameof(progressPercent), "Прогресс кампании должен быть в диапазоне 0..100.");

            ProgressPercent = progressPercent;
        }
    }

    /// <summary>
    /// Модуль сохранений и прогресса кампании.
    /// </summary>
    public class SavesAndProgressModule
    {
        /// <summary>
        /// Список доступных сохранений.
        /// </summary>
        public IReadOnlyList<CampaignSaveSnapshot> Saves => _saves.AsReadOnly();

        private readonly List<CampaignSaveSnapshot> _saves = new List<CampaignSaveSnapshot>();

        /// <summary>
        /// Создаёт новое сохранение кампании.
        /// </summary>
        /// <param name="title">Название сохранения.</param>
        /// <param name="currentMissionId">Идентификатор текущей миссии.</param>
        /// <param name="progressPercent">Прогресс кампании в процентах.</param>
        /// <returns>Созданный снимок сохранения.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если строковые параметры пустые.
        /// </exception>
        public CampaignSaveSnapshot CreateSave(string title, string currentMissionId, int progressPercent)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название сохранения не может быть пустым.", nameof(title));

            if (string.IsNullOrWhiteSpace(currentMissionId))
                throw new ArgumentException("Идентификатор текущей миссии не может быть пустым.", nameof(currentMissionId));

            string id = Guid.NewGuid().ToString("N");
            var snapshot = new CampaignSaveSnapshot(
                id,
                title,
                currentMissionId,
                DateTime.UtcNow,
                progressPercent);

            _saves.Add(snapshot);
            return snapshot;
        }

        /// <summary>
        /// Возвращает сохранение по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сохранения.</param>
        /// <returns>Найденный снимок сохранения.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если сохранение с указанным идентификатором не найдено.
        /// </exception>
        public CampaignSaveSnapshot GetSave(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор сохранения не может быть пустым.", nameof(id));

            var save = _saves.Find(s => s.Id == id);
            if (save == null)
                throw new InvalidOperationException($"Сохранение с идентификатором \"{id}\" не найдено.");

            return save;
        }

        /// <summary>
        /// Удаляет сохранение по идентификатору, если оно существует.
        /// </summary>
        /// <param name="id">Идентификатор сохранения.</param>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой.
        /// </exception>
        public void DeleteSave(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор сохранения не может быть пустым.", nameof(id));

            _saves.RemoveAll(s => s.Id == id);
        }
    }
}
