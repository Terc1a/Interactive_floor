using System;
using System.Collections;
using UnityEngine;

namespace InteractiveFloor.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public Level CurrentLevel;
        public int CurrentScore;
        public int CurrentCombo;
        public bool IsPaused;
        public float ElapsedTime;

        [Header("Events")]
        public event Action OnGameStart;
        public event Action OnGamePause;
        public event Action OnGameResume;
        public event Action OnGameEnd;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnComboChanged;

        private Coroutine gameTimerRoutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame(Level level)
        {
            CurrentLevel = level;
            CurrentScore = 0;
            CurrentCombo = 0;
            IsPaused = false;
            ElapsedTime = 0f;
            OnGameStart?.Invoke();
            if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
            gameTimerRoutine = StartCoroutine(GameTimer());
        }

        public void PauseGame()
        {
            IsPaused = true;
            OnGamePause?.Invoke();
            if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        }

        public void ResumeGame()
        {
            IsPaused = false;
            OnGameResume?.Invoke();
            if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
            gameTimerRoutine = StartCoroutine(GameTimer());
        }

        public void EndGame()
        {
            IsPaused = true;
            OnGameEnd?.Invoke();
            if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        }

        public void AddScore(int amount)
        {
            CurrentScore += amount;
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void SetCombo(int combo)
        {
            CurrentCombo = combo;
            OnComboChanged?.Invoke(CurrentCombo);
        }

        public void ResetCombo()
        {
            SetCombo(0);
        }

        private IEnumerator GameTimer()
        {
            while (!IsPaused && (CurrentLevel == null || ElapsedTime < CurrentLevel.TimeLimit || CurrentLevel.TimeLimit == 0f))
            {
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            if (!IsPaused) EndGame();
        }
    }
}
