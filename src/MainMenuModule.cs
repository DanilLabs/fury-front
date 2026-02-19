using System;
using System.Collections.Generic;

namespace FuryFront.Core.UI
{
    // Тип экрана главного меню.
    public enum MainMenuScreen
    {
        None,
        Title,
        Main,
        NewCampaign,
        LoadGame,
        Settings,
        ExitConfirmation
    }

    // Пункт меню.
    public class MenuItem
    {
        // Уникальный идентификатор пункта.
        public string Id { get; }

        // Отображаемое название.
        public string Title { get; }

        // Доступен ли пункт для выбора.
        public bool IsEnabled { get; set; }

        public MenuItem(string id, string title, bool isEnabled = true)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            IsEnabled = isEnabled;
        }
    }

    // Модуль титульного экрана и главного меню.
    public class MainMenuModule
    {
        // Текущий активный экран.
        public MainMenuScreen CurrentScreen { get; private set; }

        // Набор пунктов главного меню.
        public IReadOnlyList<MenuItem> Items => _items.AsReadOnly();

        private readonly List<MenuItem> _items = new List<MenuItem>();

        public MainMenuModule()
        {
            InitializeDefaultItems();
            CurrentScreen = MainMenuScreen.Title;
        }

        // Инициализация стандартного набора пунктов меню.
        private void InitializeDefaultItems()
        {
            _items.Clear();
            _items.Add(new MenuItem("new_campaign", "Новая кампания"));
            _items.Add(new MenuItem("load_game", "Загрузить игру"));
            _items.Add(new MenuItem("settings", "Настройки"));
            _items.Add(new MenuItem("exit", "Выход на рабочий стол"));
        }

        // Переход на основной экран меню после титульного экрана.
        public void ShowMainMenu()
        {
            CurrentScreen = MainMenuScreen.Main;
        }

        // Обработка выбора пункта меню по идентификатору.
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
