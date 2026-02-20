using System;
using System.Collections.Generic;

namespace FuryFront.Core.Tutorial
{
    /// <summary>
    /// Отдельный шаг обучения игрока.
    /// </summary>
    public class TutorialStep
    {
        /// <summary>
        /// Уникальный идентификатор шага.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Текст инструкции для игрока.
        /// </summary>
        public string Instruction { get; }

        /// <summary>
        /// Признак того, что шаг обучения выполнен.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Создаёт новый шаг обучения.
        /// </summary>
        /// <param name="id">Уникальный идентификатор шага.</param>
        /// <param name="instruction">Текст подсказки для игрока.</param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="id"/> или <paramref name="instruction"/> равны null.
        /// </exception>
        public TutorialStep(string id, string instruction)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Instruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
        }

        /// <summary>
        /// Отмечает шаг обучения как выполненный.
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
        }
    }

    /// <summary>
    /// Модуль начальной миссии и обучения управлению.
    /// </summary>
    public class TutorialModule
    {
        /// <summary>
        /// Список шагов обучения игрока.
        /// </summary>
        public IReadOnlyList<TutorialStep> Steps => _steps.AsReadOnly();

        /// <summary>
        /// Индекс текущего шага обучения.
        /// </summary>
        public int CurrentStepIndex { get; private set; } = -1;

        /// <summary>
        /// Текущий активный шаг обучения или null, если обучение не запущено.
        /// </summary>
        public TutorialStep CurrentStep =>
            (CurrentStepIndex >= 0 && CurrentStepIndex < _steps.Count)
                ? _steps[CurrentStepIndex]
                : null;

        private readonly List<TutorialStep> _steps = new List<TutorialStep>();

        /// <summary>
        /// Создаёт модуль обучения и инициализирует стандартную последовательность шагов.
        /// </summary>
        public TutorialModule()
        {
            InitializeDefaultSteps();
        }

        /// <summary>
        /// Инициализирует базовый набор шагов обучения управлению.
        /// </summary>
        private void InitializeDefaultSteps()
        {
            _steps.Clear();
            _steps.Add(new TutorialStep("move", "Освойте базовое передвижение: W, A, S, D."));
            _steps.Add(new TutorialStep("look", "Поворачивайте камеру мышью для обзора окружения."));
            _steps.Add(new TutorialStep("shoot", "Выполните выстрел по мишени левой кнопкой мыши."));
            _steps.Add(new TutorialStep("cover", "Займите укрытие и выгляните из-за него."));
            _steps.Add(new TutorialStep("grenade", "Бросьте гранату по группе целей."));
        }

        /// <summary>
        /// Запускает обучение с первого шага.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если список шагов не инициализирован.
        /// </exception>
        public void StartTutorial()
        {
            if (_steps.Count == 0)
                throw new InvalidOperationException("Список шагов обучения не инициализирован.");

            CurrentStepIndex = 0;
        }

        /// <summary>
        /// Отмечает текущий шаг выполненным и переходит к следующему.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если обучение ещё не запущено.
        /// </exception>
        public void CompleteCurrentStep()
        {
            if (CurrentStep == null)
                throw new InvalidOperationException("Обучение не запущено.");

            CurrentStep.Complete();

            if (CurrentStepIndex < _steps.Count - 1)
            {
                CurrentStepIndex++;
            }
            else
            {
                // Обучение завершено.
                CurrentStepIndex = -1;
            }
        }
    }
}
