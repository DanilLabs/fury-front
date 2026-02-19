using System;

namespace FuryFront.Core.Presentation
{
    // Состояние интерфейса игрока (HUD).
    public class HudState
    {
        // Текущее здоровье.
        public int Health { get; set; }

        // Текущая броня.
        public int Armor { get; set; }

        // Текущее количество патронов в магазине.
        public int AmmoInClip { get; set; }

        // Общее количество патронов.
        public int ReserveAmmo { get; set; }

        // Текущее активное задание.
        public string ActiveObjective { get; set; }

        // Подсказка, отображаемая игроку.
        public string HintMessage { get; set; }
    }

    // Модуль интерфейса игрока (HUD).
    public class HudModule
    {
        // Текущее состояние HUD.
        public HudState State { get; private set; }

        public HudModule()
        {
            State = new HudState
            {
                Health = 100,
                Armor = 50,
                AmmoInClip = 30,
                ReserveAmmo = 90,
                ActiveObjective = string.Empty,
                HintMessage = string.Empty
            };
        }

        // Обновить показатели здоровья и брони.
        public void UpdateHealthAndArmor(int health, int armor)
        {
            State.Health = Math.Max(0, health);
            State.Armor = Math.Max(0, armor);
        }

        // Обновить показатели боеприпасов.
        public void UpdateAmmo(int ammoInClip, int reserveAmmo)
        {
            State.AmmoInClip = Math.Max(0, ammoInClip);
            State.ReserveAmmo = Math.Max(0, reserveAmmo);
        }

        // Установить активное задание.
        public void SetActiveObjective(string objectiveText)
        {
            State.ActiveObjective = objectiveText ?? string.Empty;
        }

        // Показать краткую подсказку игроку.
        public void ShowHint(string hint)
        {
            State.HintMessage = hint ?? string.Empty;
        }

        // Очистить подсказку.
        public void ClearHint()
        {
            State.HintMessage = string.Empty;
        }
    }
}
