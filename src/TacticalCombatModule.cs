using System;

namespace FuryFront.Core.Combat
{
    // Режим ведения огня.
    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    // Состояние боевого столкновения.
    public enum CombatState
    {
        Idle,
        Engaged,
        InCover,
        Reloading
    }

    // Параметры тактического боя для игрока.
    public class TacticalCombatContext
    {
        // Текущее количество здоровья.
        public int Health { get; set; }

        // Текущее количество боеприпасов в магазине.
        public int AmmoInClip { get; set; }

        // Общее количество боеприпасов.
        public int ReserveAmmo { get; set; }

        // Текущий режим огня.
        public FireMode FireMode { get; set; }

        // Текущее состояние боя.
        public CombatState State { get; set; }
    }

    // Модуль системы свободного и тактического боя.
    public class TacticalCombatModule
    {
        // Текущее состояние боя игрока.
        public TacticalCombatContext Context { get; private set; }

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

        // Начать боевой контакт.
        public void Engage()
        {
            if (Context.Health <= 0)
                throw new InvalidOperationException("Игрок не может вступить в бой в состоянии смерти.");

            Context.State = CombatState.Engaged;
        }

        // Перейти в укрытие.
        public void TakeCover()
        {
            if (Context.State != CombatState.Engaged)
                throw new InvalidOperationException("Укрытие имеет смысл только в активном бою.");

            Context.State = CombatState.InCover;
        }

        // Выполнить выстрел.
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

        // Перезарядить оружие.
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

        // Получить урон игроком.
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
