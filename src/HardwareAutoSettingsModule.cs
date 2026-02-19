using System;

namespace FuryFront.Core.Settings
{
    // Профиль графических настроек игры.
    public class GraphicsProfile
    {
        // Название профиля (Low, Medium, High, Ultra).
        public string Name { get; set; }

        // Целевое значение FPS.
        public int TargetFps { get; set; }

        // Уровень качества текстур (0..3).
        public int TextureQuality { get; set; }

        // Уровень качества теней (0..3).
        public int ShadowQuality { get; set; }

        // Включено ли сглаживание.
        public bool AntiAliasingEnabled { get; set; }
    }

    // Результат анализа оборудования игрока.
    public class HardwareInfo
    {
        public string CpuModel { get; set; }
        public string GpuModel { get; set; }
        public int RamGb { get; set; }
        public int VramGb { get; set; }
    }

    // Модуль анализа оборудования и автоподстройки параметров.
    public class HardwareAutoSettingsModule
    {
        // Текущие данные об оборудовании.
        public HardwareInfo CurrentHardware { get; private set; }

        // Рекомендованный профиль графики.
        public GraphicsProfile RecommendedProfile { get; private set; }

        // Имитация анализа оборудования.
        public void AnalyzeHardware()
        {
            // Определение параметров оборудования игрока.
            CurrentHardware = new HardwareInfo
            {
                CpuModel = "Simulated CPU",
                GpuModel = "Simulated GPU",
                RamGb = 16,
                VramGb = 6
            };
        }

        // Подбор графического профиля на основе анализа.
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
