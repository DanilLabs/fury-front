using System;
using System.Collections.Generic;

namespace FuryFront.Core.Campaign
{
    // Снимок сохранения кампании.
    public class CampaignSaveSnapshot
    {
        // Уникальный идентификатор сохранения.
        public string Id { get; }

        // Название сохранения, отображаемое игроку.
        public string Title { get; }

        // Имя текущей миссии.
        public string CurrentMissionId { get; }

        // Время создания сохранения.
        public DateTime CreatedAt { get; }

        // Прогресс кампании в процентах.
        public int ProgressPercent { get; }

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

    // Модуль сохранений и прогресса кампании.
    public class SavesAndProgressModule
    {
        // Список доступных сохранений.
        public IReadOnlyList<CampaignSaveSnapshot> Saves => _saves.AsReadOnly();

        private readonly List<CampaignSaveSnapshot> _saves = new List<CampaignSaveSnapshot>();

        // Создать новое сохранение кампании.
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

        // Получить сохранение по идентификатору.
        public CampaignSaveSnapshot GetSave(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор сохранения не может быть пустым.", nameof(id));

            var save = _saves.Find(s => s.Id == id);
            if (save == null)
                throw new InvalidOperationException($"Сохранение с идентификатором \"{id}\" не найдено.");

            return save;
        }

        // Удалить сохранение по идентификатору.
        public void DeleteSave(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор сохранения не может быть пустым.", nameof(id));

            _saves.RemoveAll(s => s.Id == id);
        }
    }
}
