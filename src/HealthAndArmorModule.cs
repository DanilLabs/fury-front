using System;

namespace FuryFront.Core.Combat
{
    /// <summary>
    /// Состояние здоровья и брони игрока.
    /// </summary>
    public class HealthAndArmorState
    {
        /// <summary>
        /// Текущее количество очков здоровья.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Текущее количество очков брони.
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        /// Максимальное количество очков здоровья.
        /// </summary>
        public int MaxHealth { get; set; } = 100;

        /// <summary>
        /// Максимальное количество очков брони.
        /// </summary>
        public int MaxArmor { get; set; } = 100;

        /// <summary>
        /// Признак того, что игрок жив.
        /// </summary>
        public bool IsAlive => Health > 0;
    }

    /// <summary>
    /// Модуль системы здоровья, брони и повреждений.
    /// </summary>
    public class HealthAndArmorModule
    {
        /// <summary>
        /// Текущее состояние здоровья и брони игрока.
        /// </summary>
        public HealthAndArmorState State { get; }

        /// <summary>
        /// Создаёт модуль здоровья и брони с начальными значениями.
        /// </summary>
        public HealthAndArmorModule()
        {
            State = new HealthAndArmorState
            {
                Health = 100,
                Armor = 50
            };
        }

        /// <summary>
        /// Применяет входящий урон к игроку.
        /// Сначала урон поглощается бронёй, затем здоровьем.
        /// </summary>
        /// <param name="amount">Величина урона.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если величина урона отрицательна.
        /// </exception>
        public void ApplyDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Урон не может быть отрицательным.");

            if (!State.IsAlive)
                return;

            // Сначала поглощаем урон бронёй.
            int armorDamage = Math.Min(State.Armor, amount);
            State.Armor -= armorDamage;
            amount -= armorDamage;

            // Оставшийся урон уходит в здоровье.
            if (amount > 0)
            {
                State.Health -= amount;
                if (State.Health < 0)
                    State.Health = 0;
            }
        }

        /// <summary>
        /// Восстанавливает здоровье игрока на указанную величину.
        /// </summary>
        /// <param name="amount">Величина восстановления.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если величина восстановления отрицательна.
        /// </exception>
        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Восстановление не может быть отрицательным.");

            if (!State.IsAlive)
                return;

            State.Health += amount;
            if (State.Health > State.MaxHealth)
                State.Health = State.MaxHealth;
        }

        /// <summary>
        /// Восстанавливает броню игрока на указанную величину.
        /// </summary>
        /// <param name="amount">Величина восстановления брони.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если величина восстановления отрицательна.
        /// </exception>
        public void RestoreArmor(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Восстановление брони не может быть отрицательным.");

            if (!State.IsAlive)
                return;

            State.Armor += amount;
            if (State.Armor > State.MaxArmor)
                State.Armor = State.MaxArmor;
        }

        /// <summary>
        /// Полностью убивает игрока, устанавливая здоровье в ноль.
        /// </summary>
        public void Kill()
        {
            State.Health = 0;
        }
    }
}
