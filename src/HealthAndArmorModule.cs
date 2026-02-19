using System;

namespace FuryFront.Core.Combat
{
    // Тип источника урона.
    public enum DamageType
    {
        Bullet,
        Explosion,
        Melee,
        Environmental
    }

    // Параметры защиты игрока.
    public class DefenseStats
    {
        // Максимальное значение здоровья.
        public int MaxHealth { get; set; }

        // Максимальное значение брони.
        public int MaxArmor { get; set; }

        // Текущее здоровье.
        public int CurrentHealth { get; set; }

        // Текущая броня.
        public int CurrentArmor { get; set; }

        // Множитель урона по здоровью без брони.
        public float HealthDamageMultiplier { get; set; } = 1.0f;
    }

    // Модуль системы здоровья, брони и повреждений.
    public class HealthAndArmorModule
    {
        // Текущие параметры защиты игрока.
        public DefenseStats Stats { get; private set; }

        public bool IsAlive => Stats.CurrentHealth > 0;

        public HealthAndArmorModule()
        {
            Stats = new DefenseStats
            {
                MaxHealth = 100,
                MaxArmor = 50,
                CurrentHealth = 100,
                CurrentArmor = 50,
                HealthDamageMultiplier = 1.0f
            };
        }

        // Применить урон к игроку с учётом брони.
        public void ApplyDamage(int amount, DamageType type)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Урон не может быть отрицательным.");

            if (!IsAlive)
                return;

            int remainingDamage = amount;

            // Сначала снимаем урон с брони.
            if (Stats.CurrentArmor > 0)
            {
                int armorDamage = Math.Min(Stats.CurrentArmor, remainingDamage);
                Stats.CurrentArmor -= armorDamage;
                remainingDamage -= armorDamage;
            }

            // Оставшийся урон уходит в здоровье.
            if (remainingDamage > 0)
            {
                float multiplier = GetDamageMultiplier(type);
                int healthDamage = (int)(remainingDamage * multiplier);

                Stats.CurrentHealth -= healthDamage;
                if (Stats.CurrentHealth < 0)
                    Stats.CurrentHealth = 0;
            }
        }

        // Восстановление здоровья до указанного значения.
        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Восстановление не может быть отрицательным.");

            if (!IsAlive)
                return;

            Stats.CurrentHealth += amount;
            if (Stats.CurrentHealth > Stats.MaxHealth)
                Stats.CurrentHealth = Stats.MaxHealth;
        }

        // Восстановление брони до указанного значения.
        public void RestoreArmor(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Восстановление брони не может быть отрицательным.");

            Stats.CurrentArmor += amount;
            if (Stats.CurrentArmor > Stats.MaxArmor)
                Stats.CurrentArmor = Stats.MaxArmor;
        }

        // Получить множитель урона в зависимости от типа.
        private float GetDamageMultiplier(DamageType type)
        {
            switch (type)
            {
                case DamageType.Explosion:
                    return 1.3f;
                case DamageType.Melee:
                    return 1.1f;
                case DamageType.Environmental:
                    return 0.8f;
                case DamageType.Bullet:
                default:
                    return 1.0f;
            }
        }
    }
}
