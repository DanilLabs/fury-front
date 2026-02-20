using System;
using System.Collections.Generic;

namespace FuryFront.Core.UI
{
    /// <summary>
    /// Тип экрана главного меню.
    /// </summary>
    public enum MainMenuScreen
    {
        /// <summary>
        /// Экран не задан.
        /// </summary>
        None,

        /// <summary>
        /// Титульный экран.
        /// </summary>
        Title,

        /// <summary>
        /// Основное меню.
        /// </summary>
        Main,

        /// <summary>
        /// Экран начала новой кампании.
        /// </summary>
        NewCampaign,

        /// <summary>
        /// Экран загрузки сохранённой игры.
        /// </summary>
        LoadGame,

        /// <summary>
        /// Экран настроек.
        /// </summary>
        Settings,

        /// <summary>
        /// Экран подтверждения выхода из игры.
        /// </summary>
        ExitConfirmation
    }

    /// <summary>
    /// Пункт главного меню.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Уникальный идентификатор пункта меню.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Отображаемое название пункта меню.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Признак того, что пункт доступен для выбора.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Создаёт новый пункт главного меню.
        /// </summary>
        /// <param name="id">Уникальный идентификатор пункта.</param>
        /// <param name="title">Текст, отображаемый игроку.</param>
        /// <param name="isEnabled">Начальное состояние доступности пункта.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="title"/> равны null.
        /// </exception>
        public MenuItem(string id, string title, bool isEnabled = true)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            IsEnabled = isEnabled;
        }
    }

    /// <summary>
    /// Модуль титульного экрана и главного меню игры.
    /// </summary>
    public class MainMenuModule
    {
        /// <summary>
        /// Текущий активный экран главного меню.
        /// </summary>
        public MainMenuScreen CurrentScreen { get; private set; }

        /// <summary>
        /// Набор доступных пунктов главного меню.
        /// </summary>
        public IReadOnlyList<MenuItem> Items => _items.AsReadOnly();

        private readonly List<MenuItem> _items = new List<MenuItem>();

        /// <summary>
        /// Создаёт модуль главного меню и инициализирует стандартные пункты.
        /// </summary>
        public MainMenuModule()
        {
            InitializeDefaultItems();
            CurrentScreen = MainMenuScreen.Title;
        }

        /// <summary>
        /// Инициализирует стандартный набор пунктов главного меню.
        /// </summary>
        private void InitializeDefaultItems()
        {
            _items.Clear();
            _items.Add(new MenuItem("new_campaign", "Новая кампания"));
            _items.Add(new MenuItem("load_game", "Загрузить игру"));
            _items.Add(new MenuItem("settings", "Настройки"));
            _items.Add(new MenuItem("exit", "Выход на рабочий стол"));
        }

        /// <summary>
        /// Переключает интерфейс с титульного экрана на основное меню.
        /// </summary>
        public void ShowMainMenu()
        {
            CurrentScreen = MainMenuScreen.Main;
        }

        /// <summary>
        /// Обрабатывает выбор пункта меню по его идентификатору.
        /// </summary>
        /// <param name="itemId">Идентификатор выбранного пункта меню.</param>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой или состоит только из пробелов.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если пункт не найден или недоступен для выбора.
        /// </exception>
        public void SelectItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Идентификатор пункта меню не может быть пустым.", nameof(itemId));

            var item = _items.Find(i => i.Id == itemId);
            if (item == null || !item.IsEnabled)
                throw new InvalidOperationException($"Пункт меню \"{itemId}\" недоступен для выбора.");

            switch (item.Id)
            {
                case "new_campaign":
                    CurrentScreen = MainMenuScreen.NewCampaign;
                    break;
                case "load_game":
                    CurrentScreen = MainMenuScreen.LoadGame;
                    break;
                case "settings":
                    CurrentScreen = MainMenuScreen.Settings;
                    break;
                case "exit":
                    CurrentScreen = MainMenuScreen.ExitConfirmation;
                    break;
                default:
                    throw new InvalidOperationException($"Неизвестный пункт меню: \"{item.Id}\".");
            }
        }
    }
}
