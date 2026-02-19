using System;
using System.Collections.Generic;

namespace FuryFront.Core.Combat
{
    // Тип оружия.
    public enum WeaponType
    {
        AssaultRifle,
        SniperRifle,
        Shotgun,
        Pistol,
        SubmachineGun
    }

    // Редкость улучшения.
    public enum UpgradeRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    // Базовое описание оружия.
    public class Weapon
    {
        public string Id { get; }
        public string Name { get; }
        public WeaponType Type { get; }
        public int BaseDamage { get; }
        public float FireRate { get; } // выстрелов в секунду
        public int MagazineSize { get; }

        public Weapon(string id, string name, WeaponType type, int baseDamage, float fireRate, int magazineSize)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            BaseDamage = baseDamage;
            FireRate = fireRate;
            MagazineSize = magazineSize;
        }
    }

    // Описание установленного улучшения.
    public class WeaponUpgrade
    {
        public string Id { get; }
        public string Name { get; }
        public UpgradeRarity Rarity { get; }
        public int DamageBonus { get; }
        public float FireRateBonus { get; }

        public WeaponUpgrade(string id, string name, UpgradeRarity rarity, int damageBonus, float fireRateBonus)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Rarity = rarity;
            DamageBonus = damageBonus;
            FireRateBonus = fireRateBonus;
        }
    }

    // Экземпляр оружия с установленными улучшениями.
    public class EquippedWeapon
    {
        public Weapon BaseWeapon { get; }
        public IReadOnlyList<WeaponUpgrade> Upgrades => _upgrades.AsReadOnly();

        private readonly List<WeaponUpgrade> _upgrades = new List<WeaponUpgrade>();

        public EquippedWeapon(Weapon baseWeapon)
        {
            BaseWeapon = baseWeapon ?? throw new ArgumentNullException(nameof(baseWeapon));
        }

        // Добавить улучшение к оружию.
        public void AddUpgrade(WeaponUpgrade upgrade)
        {
            if (upgrade == null)
                throw new ArgumentNullException(nameof(upgrade));

            _upgrades.Add(upgrade);
        }

        // Рассчитать итоговый урон с учётом улучшений.
        public int GetTotalDamage()
        {
            int total = BaseWeapon.BaseDamage;
            foreach (var upgrade in _upgrades)
            {
                total += upgrade.DamageBonus;
            }
            return total;
        }

        // Рассчитать итоговую скорострельность.
        public float GetTotalFireRate()
        {
            float total = BaseWeapon.FireRate;
            foreach (var upgrade in _upgrades)
            {
                total += upgrade.FireRateBonus;
            }
            return total;
        }
    }

    // Модуль вооружения и улучшений.
    public class WeaponsAndUpgradesModule
    {
        // Доступный арсенал оружия.
        public IReadOnlyDictionary<string, Weapon> Arsenal => _arsenal;

        private readonly Dictionary<string, Weapon> _arsenal = new Dictionary<string, Weapon>();

        public WeaponsAndUpgradesModule()
        {
            InitializeDefaultArsenal();
        }

        // Инициализация базового набора оружия.
        private void InitializeDefaultArsenal()
        {
            _arsenal.Clear();

            _arsenal["rifle_ak"] = new Weapon(
                "rifle_ak",
                "Штурмовая винтовка АК",
                WeaponType.AssaultRifle,
                baseDamage: 30,
                fireRate: 9.0f,
                magazineSize: 30);

            _arsenal["pistol_std"] = new Weapon(
                "pistol_std",
                "Служебный пистолет",
                WeaponType.Pistol,
                baseDamage: 20,
                fireRate: 4.0f,
                magazineSize: 15);

            _arsenal["shotgun_pump"] = new Weapon(
                "shotgun_pump",
                "Помповый дробовик",
                WeaponType.Shotgun,
                baseDamage: 80,
                fireRate: 1.0f,
                magazineSize: 8);
        }

        // Получить оружие по идентификатору.
        public Weapon GetWeapon(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Идентификатор оружия не может быть пустым.", nameof(id));

            if (!_arsenal.TryGetValue(id, out var weapon))
                throw new InvalidOperationException($"Оружие с идентификатором \"{id}\" не найдено в арсенале.");

            return weapon;
        }
    }
}
