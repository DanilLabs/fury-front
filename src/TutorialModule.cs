using System;
using System.Collections.Generic;

namespace FuryFront.Core.Tutorial
{
    // Этап обучения, через который проходит игрок.
    public class TutorialStep
    {
        // Уникальный идентификатор шага.
        public string Id { get; }

        // Текст инструкции для игрока.
        public string Instruction { get; }

        // Флаг, выполнен ли шаг.
        public bool IsCompleted { get; private set; }

        public TutorialStep(string id, string instruction)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Instruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
        }

        // Отметить текущий шаг как выполненный.
        public void Complete()
        {
            IsCompleted = true;
        }
    }

    // Модуль начальной миссии и обучения управлению.
    public class TutorialModule
    {
        // Список шагов обучения.
        public IReadOnlyList<TutorialStep> Steps => _steps.AsReadOnly();

        // Индекс текущего шага.
        public int CurrentStepIndex { get; private set; } = -1;

        // Текущий активный шаг обучения.
        public TutorialStep CurrentStep =>
            (CurrentStepIndex >= 0 && CurrentStepIndex < _steps.Count)
                ? _steps[CurrentStepIndex]
                : null;

        private readonly List<TutorialStep> _steps = new List<TutorialStep>();

        public TutorialModule()
        {
            InitializeDefaultSteps();
        }

        // Инициализация базовой последовательности шагов обучения.
        private void InitializeDefaultSteps()
        {
            _steps.Clear();
            _steps.Add(new TutorialStep("move", "Освойте базовое передвижение: W, A, S, D."));
            _steps.Add(new TutorialStep("look", "Поворачивайте камеру мышью для обзора окружения."));
            _steps.Add(new TutorialStep("shoot", "Выполните выстрел по мишени левой кнопкой мыши."));
            _steps.Add(new TutorialStep("cover", "Займите укрытие и выгляните из-за него."));
            _steps.Add(new TutorialStep("grenade", "Бросьте гранату по группе целей."));
        }

        // Запуск обучения с первого шага.
        public void StartTutorial()
        {
            if (_steps.Count == 0)
                throw new InvalidOperationException("Список шагов обучения не инициализирован.");

            CurrentStepIndex = 0;
        }

        // Отметить текущий шаг выполненным и перейти к следующему.
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
