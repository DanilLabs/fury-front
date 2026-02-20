using System;

namespace FuryFront.Core.Presentation
{
    /// <summary>
    /// Состояние интерфейса игрока (HUD).
    /// </summary>
    public class HudState
    {
        /// <summary>
        /// Текущее здоровье игрока.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Текущая броня игрока.
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        /// Количество патронов в магазине.
        /// </summary>
        public int AmmoInClip { get; set; }

        /// <summary>
        /// Общее количество патронов.
        /// </summary>
        public int ReserveAmmo { get; set; }

        /// <summary>
        /// Текст активного задания.
        /// </summary>
        public string ActiveObjective { get; set; }

        /// <summary>
        /// Текст краткой подсказки игроку.
        /// </summary>
        public string HintMessage { get; set; }
    }

    /// <summary>
    /// Модуль интерфейса игрока (HUD).
    /// </summary>
    public class HudModule
    {
        /// <summary>
        /// Текущее состояние HUD.
        /// </summary>
        public HudState State { get; private set; }

        /// <summary>
        /// Создаёт модуль HUD и инициализирует начальные значения.
        /// </summary>
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

        /// <summary>
        /// Обновляет показатели здоровья и брони в HUD.
        /// </summary>
        /// <param name="health">Новое значение здоровья.</param>
        /// <param name="armor">Новое значение брони.</param>
        public void UpdateHealthAndArmor(int health, int armor)
        {
            State.Health = Math.Max(0, health);
            State.Armor = Math.Max(0, armor);
        }

        /// <summary>
        /// Обновляет показатели боеприпасов в HUD.
        /// </summary>
        /// <param name="ammoInClip">Патроны в магазине.</param>
        /// <param name="reserveAmmo">Патроны в запасе.</param>
        public void UpdateAmmo(int ammoInClip, int reserveAmmo)
        {
            State.AmmoInClip = Math.Max(0, ammoInClip);
            State.ReserveAmmo = Math.Max(0, reserveAmmo);
        }

        /// <summary>
        /// Устанавливает активное задание для отображения в HUD.
        /// </summary>
        /// <param name="objectiveText">Текст активного задания.</param>
        public void SetActiveObjective(string objectiveText)
        {
            State.ActiveObjective = objectiveText ?? string.Empty;
        }

        /// <summary>
        /// Показывает краткую подсказку игроку.
        /// </summary>
        /// <param name="hint">Текст подсказки.</param>
        public void ShowHint(string hint)
        {
            State.HintMessage = hint ?? string.Empty;
        }

        /// <summary>
        /// Очищает текст подсказки в HUD.
        /// </summary>
        public void ClearHint()
        {
            State.HintMessage = string.Empty;
        }
    }
}
