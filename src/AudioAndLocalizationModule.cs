using System;
using System.Collections.Generic;

namespace FuryFront.Core.Presentation
{
    /// <summary>
    /// Тип звукового события.
    /// </summary>
    public enum SoundEventType
    {
        /// <summary>
        /// Звуки пользовательского интерфейса.
        /// </summary>
        UiClick,

        /// <summary>
        /// Выстрел из оружия.
        /// </summary>
        GunShot,

        /// <summary>
        /// Взрыв.
        /// </summary>
        Explosion,

        /// <summary>
        /// Шаги.
        /// </summary>
        Footstep,

        /// <summary>
        /// Реплика персонажа.
        /// </summary>
        VoiceLine,

        /// <summary>
        /// Фоновый звук окружения.
        /// </summary>
        Ambient
    }

    /// <summary>
    /// Описание звукового события.
    /// </summary>
    public class SoundEvent
    {
        /// <summary>
        /// Уникальный идентификатор звукового события.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Тип звукового события.
        /// </summary>
        public SoundEventType Type { get; }

        /// <summary>
        /// Путь к звуковому ресурсу.
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// Создаёт описание звукового события.
        /// </summary>
        /// <param name="id">Уникальный идентификатор события.</param>
        /// <param name="type">Тип звукового события.</param>
        /// <param name="assetPath">Путь к звуковому файлу.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="assetPath"/> равны null.
        /// </exception>
        public SoundEvent(string id, SoundEventType type, string assetPath)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type;
            AssetPath = assetPath ?? throw new ArgumentNullException(nameof(assetPath));
        }
    }

    /// <summary>
    /// Таблица локализованных строк.
    /// </summary>
    public class LocalizationTable
    {
        /// <summary>
        /// Код языка (например, ru-RU, en-US).
        /// </summary>
        public string LanguageCode { get; }

        private readonly Dictionary<string, string> _entries = new Dictionary<string, string>();

        /// <summary>
        /// Создаёт таблицу локализации для указанного языка.
        /// </summary>
        /// <param name="languageCode">Код языка.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="languageCode"/> равен null.
        /// </exception>
        public LocalizationTable(string languageCode)
        {
            LanguageCode = languageCode ?? throw new ArgumentNullException(nameof(languageCode));
        }

        /// <summary>
        /// Добавляет или обновляет локализованную строку.
        /// </summary>
        /// <param name="key">Ключ локализации.</param>
        /// <param name="value">Локализованное значение.</param>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если ключ пустой.
        /// </exception>
        public void SetEntry(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ локализации не может быть пустым.", nameof(key));

            _entries[key] = value ?? string.Empty;
        }

        /// <summary>
        /// Возвращает локализованную строку по ключу.
        /// Если ключ не найден, возвращается сам ключ.
        /// </summary>
        /// <param name="key">Ключ локализации.</param>
        /// <returns>Локализованное значение или ключ.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если ключ пустой.
        /// </exception>
        public string GetEntry(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ локализации не может быть пустым.", nameof(key));

            return _entries.TryGetValue(key, out var value) ? value : key;
        }
    }

    /// <summary>
    /// Модуль звука, музыки и локализации.
    /// </summary>
    public class AudioAndLocalizationModule
    {
        /// <summary>
        /// Зарегистрированные звуковые события.
        /// </summary>
        public IReadOnlyDictionary<string, SoundEvent> SoundEvents => _soundEvents;

        /// <summary>
        /// Текущая активная таблица локализации.
        /// </summary>
        public LocalizationTable CurrentLocalization { get; private set; }

        private readonly Dictionary<string, SoundEvent> _soundEvents =
            new Dictionary<string, SoundEvent>();

        /// <summary>
        /// Создаёт модуль звука и локализации, инициализируя значения по умолчанию.
        /// </summary>
        public AudioAndLocalizationModule()
        {
            InitializeDefaultSounds();
            InitializeDefaultLocalization();
        }

        /// <summary>
        /// Инициализирует базовый набор звуковых событий.
        /// </summary>
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

        /// <summary>
        /// Инициализирует локализацию по умолчанию (например, ru-RU).
        /// </summary>
        private void InitializeDefaultLocalization()
        {
            var table = new LocalizationTable("ru-RU");
            table.SetEntry("menu_new_campaign", "Новая кампания");
            table.SetEntry("menu_load_game", "Загрузить игру");
            table.SetEntry("menu_settings", "Настройки");
            table.SetEntry("menu_exit", "Выход на рабочий стол");

            CurrentLocalization = table;
        }

        /// <summary>
        /// Устанавливает указанную таблицу локализации активной.
        /// </summary>
        /// <param name="table">Таблица локализованных строк.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="table"/> равна null.
        /// </exception>
        public void SetLocalization(LocalizationTable table)
        {
            CurrentLocalization = table ?? throw new ArgumentNullException(nameof(table));
        }

        /// <summary>
        /// Возвращает звуковое событие по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>Описание звукового события.</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если событие с указанным идентификатором не найдено.
        /// </exception>
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
