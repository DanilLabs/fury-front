using System;
using System.Collections.Generic;

namespace FuryFront.Core.Presentation
{
    // Тип звукового события.
    public enum SoundEventType
    {
        UiClick,
        GunShot,
        Explosion,
        Footstep,
        VoiceLine,
        Ambient
    }

    // Описание звукового события.
    public class SoundEvent
    {
        public string Id { get; }
        public SoundEventType Type { get; }
        public string AssetPath { get; }

        public SoundEvent(string id, SoundEventType type, string assetPath)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type;
            AssetPath = assetPath ?? throw new ArgumentNullException(nameof(assetPath));
        }
    }

    // Таблица локализованных строк.
    public class LocalizationTable
    {
        public string LanguageCode { get; }

        private readonly Dictionary<string, string> _entries = new Dictionary<string, string>();

        public LocalizationTable(string languageCode)
        {
            LanguageCode = languageCode ?? throw new ArgumentNullException(nameof(languageCode));
        }

        // Добавить или обновить локализованную строку.
        public void SetEntry(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ локализации не может быть пустым.", nameof(key));

            _entries[key] = value ?? string.Empty;
        }

        // Получить локализованную строку по ключу.
        public string GetEntry(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ локализации не может быть пустым.", nameof(key));

            return _entries.TryGetValue(key, out var value) ? value : key;
        }
    }

    // Модуль звука, музыки и локализации.
    public class AudioAndLocalizationModule
    {
        // Зарегистрированные звуковые события.
        public IReadOnlyDictionary<string, SoundEvent> SoundEvents => _soundEvents;

        // Активная таблица локализации.
        public LocalizationTable CurrentLocalization { get; private set; }

        private readonly Dictionary<string, SoundEvent> _soundEvents = new Dictionary<string, SoundEvent>();

        public AudioAndLocalizationModule()
        {
            InitializeDefaultSounds();
            InitializeDefaultLocalization();
        }

        // Инициализация базового набора звуков.
        private void InitializeDefaultSounds()
        {
            _soundEvents.Clear();

            _soundEvents["ui_click"] = new SoundEvent(
                "ui_click",
                SoundEventType.UiClick,
                "audio/ui/click_01.ogg");

            _soundEvents["gun_shot_rifle"] = new SoundEvent(
                "gun_shot_rifle",
                SoundEventType.GunShot,
                "audio/weapons/rifle_shot_01.ogg");

            _soundEvents["explosion_grenade"] = new SoundEvent(
                "explosion_grenade",
                SoundEventType.Explosion,
                "audio/explosions/grenade_01.ogg");
        }

        // Инициализация локализации по умолчанию (например, ru-RU).
        private void InitializeDefaultLocalization()
        {
            var table = new LocalizationTable("ru-RU");
            table.SetEntry("menu_new_campaign", "Новая кампания");
            table.SetEntry("menu_load_game", "Загрузить игру");
            table.SetEntry("menu_settings", "Настройки");
            table.SetEntry("menu_exit", "Выход на рабочий стол");

            CurrentLocalization = table;
        }

        // Сменить активную таблицу локализации.
        public void SetLocalization(LocalizationTable table)
        {
            CurrentLocalization = table ?? throw new ArgumentNullException(nameof(table));
        }

        // Получить звуковое событие по идентификатору.
        public SoundEvent GetSoundEvent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор звукового события не может быть пустым.", nameof(id));

            if (!_soundEvents.TryGetValue(id, out var soundEvent))
                throw new InvalidOperationException($"Звуковое событие \"{id}\" не найдено.");

            return soundEvent;
        }
    }
}
