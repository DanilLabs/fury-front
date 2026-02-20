using System;

namespace FuryFront.Core.Combat
{
    /// <summary>
    /// Режим ведения огня.
    /// </summary>
    public enum FireMode
    {
        /// <summary>
        /// Одиночные выстрелы.
        /// </summary>
        Single,

        /// <summary>
        /// Очередь из нескольких выстрелов.
        /// </summary>
        Burst,

        /// <summary>
        /// Автоматический огонь.
        /// </summary>
        Auto
    }

    /// <summary>
    /// Состояние боевого столкновения.
    /// </summary>
    public enum CombatState
    {
        /// <summary>
        /// Бой не ведётся.
        /// </summary>
        Idle,

        /// <summary>
        /// Игрок вовлечён в бой.
        /// </summary>
        Engaged,

        /// <summary>
        /// Игрок находится в укрытии.
        /// </summary>
        InCover,

        /// <summary>
        /// Игрок выполняет перезарядку.
        /// </summary>
        Reloading
    }

    /// <summary>
    /// Параметры тактического боя для игрока.
    /// </summary>
    public class TacticalCombatContext
    {
        /// <summary>
        /// Текущее количество здоровья.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Текущее количество боеприпасов в магазине.
        /// </summary>
        public int AmmoInClip { get; set; }

        /// <summary>
        /// Общее количество боеприпасов.
        /// </summary>
        public int ReserveAmmo { get; set; }

        /// <summary>
        /// Текущий режим огня.
        /// </summary>
        public FireMode FireMode { get; set; }

        /// <summary>
        /// Текущее состояние боя.
        /// </summary>
        public CombatState State { get; set; }
    }

    /// <summary>
    /// Модуль системы свободного и тактического боя.
    /// </summary>
    public class TacticalCombatModule
    {
        /// <summary>
        /// Текущее состояние боя игрока.
        /// </summary>
        public TacticalCombatContext Context { get; private set; }

        /// <summary>
        /// Создаёт модуль тактического боя и инициализирует базовый контекст.
        /// </summary>
        public TacticalCombatModule()
        {
            Context = new TacticalCombatContext
            {
                Health = 100,
                AmmoInClip = 30,
                ReserveAmmo = 90,
                FireMode = FireMode.Single,
                State = CombatState.Idle
            };
        }

        /// <summary>
        /// Переводит игрока в состояние активного боя.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если здоровье игрока равно нулю.
        /// </exception>
        public void Engage()
        {
            if (Context.Health <= 0)
                throw new InvalidOperationException("Игрок не может вступить в бой в состоянии смерти.");

            Context.State = CombatState.Engaged;
        }

        /// <summary>
        /// Переводит игрока в укрытие.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если игрок не находится в активном бою.
        /// </exception>
        public void TakeCover()
        {
            if (Context.State != CombatState.Engaged)
                throw new InvalidOperationException("Укрытие имеет смысл только в активном бою.");

            Context.State = CombatState.InCover;
        }

        /// <summary>
        /// Выполняет выстрел из текущего оружия.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если игрок не в бою или магазин пуст.
        /// </exception>
        public void Fire()
        {
            if (Context.State == CombatState.Idle)
                throw new InvalidOperationException("Стрельба недоступна вне боевого столкновения.");

            if (Context.AmmoInClip <= 0)
                throw new InvalidOperationException("Магазин пуст. Необходимо перезарядиться.");

            Context.AmmoInClip--;

            if (Context.AmmoInClip == 0 && Context.ReserveAmmo > 0)
            {
                Reload();
            }
        }

        /// <summary>
        /// Перезаряжает оружие из запаса боеприпасов.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если нет боеприпасов для перезарядки.
        /// </exception>
        public void Reload()
        {
            if (Context.ReserveAmmo <= 0)
                throw new InvalidOperationException("Нет боеприпасов для перезарядки.");

            Context.State = CombatState.Reloading;

            int needed = 30 - Context.AmmoInClip;
            int toLoad = Math.Min(needed, Context.ReserveAmmo);

            Context.AmmoInClip += toLoad;
            Context.ReserveAmmo -= toLoad;

            Context.State = CombatState.Engaged;
        }

        /// <summary>
        /// Применяет к игроку входящий урон.
        /// </summary>
        /// <param name="amount">Величина урона.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если величина урона отрицательна.
        /// </exception>
        public void ApplyDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Урон не может быть отрицательным.");

            if (Context.Health <= 0)
                return;

            Context.Health -= amount;

            if (Context.Health < 0)
                Context.Health = 0;
        }
    }
}
