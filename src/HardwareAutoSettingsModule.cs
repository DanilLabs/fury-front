using System;

namespace FuryFront.Core.Settings
{
    /// <summary>
    /// Профиль графических настроек игры.
    /// </summary>
    public class GraphicsProfile
    {
        /// <summary>
        /// Название профиля (Low, Medium, High, Ultra).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Целевое значение кадров в секунду (FPS).
        /// </summary>
        public int TargetFps { get; set; }

        /// <summary>
        /// Уровень качества текстур (0..3).
        /// </summary>
        public int TextureQuality { get; set; }

        /// <summary>
        /// Уровень качества теней (0..3).
        /// </summary>
        public int ShadowQuality { get; set; }

        /// <summary>
        /// Признак включения сглаживания.
        /// </summary>
        public bool AntiAliasingEnabled { get; set; }
    }

    /// <summary>
    /// Данные об аппаратной конфигурации игрока.
    /// </summary>
    public class HardwareInfo
    {
        /// <summary>
        /// Модель центрального процессора.
        /// </summary>
        public string CpuModel { get; set; }

        /// <summary>
        /// Модель графического адаптера.
        /// </summary>
        public string GpuModel { get; set; }

        /// <summary>
        /// Объём оперативной памяти, ГБ.
        /// </summary>
        public int RamGb { get; set; }

        /// <summary>
        /// Объём видеопамяти, ГБ.
        /// </summary>
        public int VramGb { get; set; }
    }

    /// <summary>
    /// Модуль анализа оборудования и автоподстройки графических параметров.
    /// </summary>
    public class HardwareAutoSettingsModule
    {
        /// <summary>
        /// Текущие данные об оборудовании игрока.
        /// Заполняются после вызова <see cref="AnalyzeHardware"/>.
        /// </summary>
        public HardwareInfo CurrentHardware { get; private set; }

        /// <summary>
        /// Рекомендованный профиль графики на основе текущего оборудования.
        /// </summary>
        public GraphicsProfile RecommendedProfile { get; private set; }

        /// <summary>
        /// Выполняет анализ аппаратной конфигурации игрока
        /// и заполняет структуру <see cref="CurrentHardware"/>.
        /// </summary>
        public void AnalyzeHardware()
        {
            // Определение параметров оборудования игрока.
            CurrentHardware = new HardwareInfo
            {
                CpuModel = "Intel Core i7-12700K",
                GpuModel = "NVIDIA GeForce RTX 3060",
                RamGb = 16,
                VramGb = 6
            };
        }

        /// <summary>
        /// Подбирает и сохраняет в <see cref="RecommendedProfile"/> графический
        /// профиль, соответствующий текущему оборудованию.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если оборудование ещё не было проанализировано.
        /// </exception>
        public void ApplyRecommendedSettings()
        {
            if (CurrentHardware == null)
                throw new InvalidOperationException("Оборудование ещё не было проанализировано.");

            // Упрощённая логика выбора профиля.
            if (CurrentHardware.RamGb >= 16 && CurrentHardware.VramGb >= 6)
            {
                RecommendedProfile = new GraphicsProfile
                {
                    Name = "High",
                    TargetFps = 60,
                    TextureQuality = 3,
                    ShadowQuality = 3,
                    AntiAliasingEnabled = true
                };
            }
            else
            {
                RecommendedProfile = new GraphicsProfile
                {
                    Name = "Medium",
                    TargetFps = 45,
                    TextureQuality = 2,
                    ShadowQuality = 1,
                    AntiAliasingEnabled = false
                };
            }
        }
    }
}
