using System;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveFloor.Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels")]
        public GameObject MainMenuPanel;
        public GameObject GamePanel;
        public GameObject PausePanel;
        public GameObject GameOverPanel;
        public GameObject EditorPanel;

        [Header("UI Elements")]
        public Text ScoreText;
        public Text ComboText;
        public Text TimerText;
        public Button PauseButton;
        public Button ResumeButton;
        public Button MainMenuButton;

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

        private void Start()
        {
            if (PauseButton != null) PauseButton.onClick.AddListener(OnPauseClicked);
            if (ResumeButton != null) ResumeButton.onClick.AddListener(OnResumeClicked);
            if (MainMenuButton != null) MainMenuButton.onClick.AddListener(OnMainMenuClicked);
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            SetAllPanelsInactive();
            if (MainMenuPanel != null) MainMenuPanel.SetActive(true);
        }
        public void ShowGame()
        {
            SetAllPanelsInactive();
            if (GamePanel != null) GamePanel.SetActive(true);
        }
        public void ShowPause()
        {
            if (PausePanel != null) PausePanel.SetActive(true);
        }
        public void HidePause()
        {
            if (PausePanel != null) PausePanel.SetActive(false);
        }
        public void ShowGameOver()
        {
            SetAllPanelsInactive();
            if (GameOverPanel != null) GameOverPanel.SetActive(true);
        }
        public void ShowEditor()
        {
            SetAllPanelsInactive();
            if (EditorPanel != null) EditorPanel.SetActive(true);
        }
        private void SetAllPanelsInactive()
        {
            if (MainMenuPanel != null) MainMenuPanel.SetActive(false);
            if (GamePanel != null) GamePanel.SetActive(false);
            if (PausePanel != null) PausePanel.SetActive(false);
            if (GameOverPanel != null) GameOverPanel.SetActive(false);
            if (EditorPanel != null) EditorPanel.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            if (ScoreText != null) ScoreText.text = $"Score: {score}";
        }
        public void UpdateCombo(int combo)
        {
            if (ComboText != null) ComboText.text = $"Combo: {combo}";
        }
        public void UpdateTimer(float time)
        {
            if (TimerText != null) TimerText.text = $"Time: {time:F1}";
        }

        private void OnPauseClicked()
        {
            GameManager.Instance?.PauseGame();
            ShowPause();
        }
        private void OnResumeClicked()
        {
            GameManager.Instance?.ResumeGame();
            HidePause();
        }
        private void OnMainMenuClicked()
        {
            GameManager.Instance?.EndGame();
            ShowMainMenu();
        }
    }
}
