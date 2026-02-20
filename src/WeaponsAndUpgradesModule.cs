using System;
using System.Collections.Generic;

namespace FuryFront.Core.Combat
{
    /// <summary>
    /// Тип оружия.
    /// </summary>
    public enum WeaponType
    {
        /// <summary>
        /// Пистолет.
        /// </summary>
        Pistol,

        /// <summary>
        /// Штурмовая винтовка.
        /// </summary>
        Rifle,

        /// <summary>
        /// Пулемёт.
        /// </summary>
        MachineGun,

        /// <summary>
        /// Снайперская винтовка.
        /// </summary>
        SniperRifle
    }

    /// <summary>
    /// Описание оружия игрока.
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// Уникальный идентификатор оружия.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Отображаемое название оружия.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Тип оружия.
        /// </summary>
        public WeaponType Type { get; }

        /// <summary>
        /// Базовый урон от одного выстрела.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Вместимость магазина.
        /// </summary>
        public int ClipSize { get; set; }

        /// <summary>
        /// Скорострельность (выстрелов в минуту).
        /// </summary>
        public int RateOfFire { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр оружия.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="name">Название оружия.</param>
        /// <param name="type">Тип оружия.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="name"/> равны null.
        /// </exception>
        public Weapon(string id, string name, WeaponType type)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
        }
    }

    /// <summary>
    /// Улучшение оружия (модернизация).
    /// </summary>
    public class WeaponUpgrade
    {
        /// <summary>
        /// Уникальный идентификатор улучшения.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Отображаемое название улучшения.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Прибавка к урону.
        /// </summary>
        public int DamageBonus { get; set; }

        /// <summary>
        /// Прибавка к ёмкости магазина.
        /// </summary>
        public int ClipSizeBonus { get; set; }

        /// <summary>
        /// Создаёт новое улучшение оружия.
        /// </summary>
        /// <param name="id">Уникальный идентификатор улучшения.</param>
        /// <param name="title">Название улучшения.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="title"/> равны null.
        /// </exception>
        public WeaponUpgrade(string id, string title)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }
    }

    /// <summary>
    /// Модуль вооружения и улучшений.
    /// </summary>
    public class WeaponsAndUpgradesModule
    {
        /// <summary>
        /// Текущее активное оружие игрока.
        /// </summary>
        public Weapon CurrentWeapon { get; private set; }

        /// <summary>
        /// Список доступного игроку оружия.
        /// </summary>
        public IReadOnlyList<Weapon> Weapons => _weapons.AsReadOnly();

        private readonly List<Weapon> _weapons = new List<Weapon>();

        /// <summary>
        /// Добавляет оружие в инвентарь игрока.
        /// </summary>
        /// <param name="weapon">Экземпляр оружия.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="weapon"/> равен null.
        /// </exception>
        public void AddWeapon(Weapon weapon)
        {
            if (weapon == null)
                throw new ArgumentNullException(nameof(weapon));

            _weapons.Add(weapon);

            if (CurrentWeapon == null)
                CurrentWeapon = weapon;
        }

        /// <summary>
        /// Устанавливает активное оружие по идентификатору.
        /// </summary>
        /// <param name="weaponId">Идентификатор оружия.</param>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если идентификатор пустой.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если оружие с таким идентификатором не найдено.
        /// </exception>
        public void EquipWeapon(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId))
                throw new ArgumentException("Идентификатор оружия не может быть пустым.", nameof(weaponId));

            var weapon = _weapons.Find(w => w.Id == weaponId);
            if (weapon == null)
                throw new InvalidOperationException($"Оружие с идентификатором \"{weaponId}\" не найдено.");

            CurrentWeapon = weapon;
        }

        /// <summary>
        /// Применяет улучшение к текущему оружию.
        /// </summary>
        /// <param name="upgrade">Улучшение, которое необходимо применить.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="upgrade"/> равен null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если активное оружие не задано.
        /// </exception>
        public void ApplyUpgrade(WeaponUpgrade upgrade)
        {
            if (upgrade == null)
                throw new ArgumentNullException(nameof(upgrade));

            if (CurrentWeapon == null)
                throw new InvalidOperationException("Активное оружие не задано.");

            CurrentWeapon.Damage += upgrade.DamageBonus;
            CurrentWeapon.ClipSize += upgrade.ClipSizeBonus;
        }
    }
}
