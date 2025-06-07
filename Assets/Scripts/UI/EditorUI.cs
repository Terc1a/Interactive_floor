using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteractiveFloor.Core
{
    public class EditorUI : MonoBehaviour
    {
        [Header("UI References")]
        public InputField LevelNameField;
        public InputField DescriptionField;
        public Dropdown DifficultyDropdown;
        public Button SaveButton;
        public Button LoadButton;
        public Button NewLevelButton;
        public Transform TileGridParent;
        public GameObject TileButtonPrefab;

        [Header("Current State")]
        public Level CurrentLevel;
        private List<GameObject> tileButtons = new List<GameObject>();

        private void Start()
        {
            if (SaveButton != null) SaveButton.onClick.AddListener(OnSaveClicked);
            if (LoadButton != null) LoadButton.onClick.AddListener(OnLoadClicked);
            if (NewLevelButton != null) NewLevelButton.onClick.AddListener(OnNewLevelClicked);
        }

        public void SetLevel(Level level)
        {
            CurrentLevel = level;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (CurrentLevel == null) return;
            if (LevelNameField != null) LevelNameField.text = CurrentLevel.LevelName;
            if (DescriptionField != null) DescriptionField.text = CurrentLevel.Description;
            if (DifficultyDropdown != null) DifficultyDropdown.value = Mathf.Clamp(CurrentLevel.Difficulty - 1, 0, DifficultyDropdown.options.Count - 1);
            RenderTiles();
        }

        private void RenderTiles()
        {
            foreach (var btn in tileButtons)
                Destroy(btn);
            tileButtons.Clear();

            if (CurrentLevel == null || TileGridParent == null || TileButtonPrefab == null) return;
            foreach (var tile in CurrentLevel.Tiles)
            {
                GameObject btn = Instantiate(TileButtonPrefab, TileGridParent);
                btn.GetComponentInChildren<Text>().text = tile.Position.ToString();
                btn.GetComponent<Image>().color = tile.Color;
                btn.GetComponent<Button>().onClick.AddListener(() => OnTileButtonClicked(tile));
                tileButtons.Add(btn);
            }
        }

        private void OnTileButtonClicked(LevelTile tile)
        {
            // Открыть редактор тайла, подсветить, изменить цвет и пр.
            Debug.Log($"Tile clicked: {tile.Position} type: {tile.Type}");
            // Реализовать здесь редактор свойств выбранной плитки
        }

        private void OnSaveClicked()
        {
            if (CurrentLevel == null) return;
            CurrentLevel.LevelName = LevelNameField.text;
            CurrentLevel.Description = DescriptionField.text;
            CurrentLevel.Difficulty = DifficultyDropdown.value + 1;
            // Добавь здесь сохранение уровня (например, сериализация в JSON, вызов APIManager)
            Debug.Log("Level saved");
        }

        private void OnLoadClicked()
        {
            // Добавь здесь логику загрузки уровня (например, открытие файла, вызов APIManager)
            Debug.Log("Load level clicked");
        }

        private void OnNewLevelClicked()
        {
            SetLevel(new Level());
            Debug.Log("New level created");
        }
    }
}
