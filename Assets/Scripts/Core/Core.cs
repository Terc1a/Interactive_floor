// GameManager.cs
using UnityEngine;
using InteractiveFloor.Core;
using InteractiveFloor.GameModes;
using UnityEngine.UI;

namespace InteractiveFloor.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        public GameMode CurrentMode { get; private set; }

        [Header("Display Canvases")]
        public Canvas playerUICanvas;
        public Canvas adminUICanvas;

        [Header("Admin UI References")]
        public Button nextLevelButton;
        public Button prevLevelButton;
        public Dropdown levelDropdown;
        public Toggle continueToggle;
        public InputField livesInput;
        public InputField blueTilesInput;
        public InputField timeInput;
        public Slider speedSlider;

        private bool allowContinue = false;
        private int currentLives = 3;
        private int requiredBlueTiles = 5;
        private float levelTime = 60f;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            // Activate additional display if available
            Display.displays[0].Activate();
            if (Display.displays.Length > 1) {
                Display.displays[1].Activate();
            }

            SetupAdminUI();
            ShowMainMenu();
        }

        private void SetupAdminUI() {
            nextLevelButton.onClick.AddListener(OnNextLevel);
            prevLevelButton.onClick.AddListener(OnPrevLevel);
            levelDropdown.onValueChanged.AddListener(OnLevelDropdownChanged);
            continueToggle.onValueChanged.AddListener(val => allowContinue = val);
            livesInput.onEndEdit.AddListener(val => SetLives(int.Parse(val)));
            blueTilesInput.onEndEdit.AddListener(val => SetRequiredBlue(int.Parse(val)));
            timeInput.onEndEdit.AddListener(val => SetLevelTime(float.Parse(val)));
            speedSlider.onValueChanged.AddListener(val => SetGameSpeed(val));
        }

        public void ShowMainMenu() {
            // TODO: activate main menu UI
        }
        public void ShowGameUI() {
            // TODO: activate game HUD
        }
        public void ShowAdminUI() {
            // TODO: activate admin panel
        }
        public void ShowLevelEditor() {
            // TODO: open level editor scene
        }

        public void StartGame(GameMode mode) {
            CurrentMode = mode;
            CurrentMode.StartMode();
            ShowGameUI();
        }
        public void RestartLevel() {
            CurrentMode.RestartMode();
        }
        public void LoadLevel(int index) {
            CurrentMode?.StopMode();
            CurrentMode.LoadLevel(index);
            CurrentMode.StartMode();
        }
        public void EndGame() {
            CurrentMode?.StopMode();
            ShowMainMenu();
        }

        private void OnNextLevel() {
            LoadLevel(CurrentMode.CurrentLevelIndex + 1);
        }
        private void OnPrevLevel() {
            LoadLevel(CurrentMode.CurrentLevelIndex - 1);
        }
        private void OnLevelDropdownChanged(int idx) {
            LoadLevel(idx);
        }

        public void SetLives(int count) {
            currentLives = count;
            // TODO: update UI
        }
        public void SetRequiredBlue(int count) {
            requiredBlueTiles = count;
        }
        public void SetLevelTime(float time) {
            levelTime = time;
        }
        public void SetGameSpeed(float speed) {
            Time.timeScale = speed;
        }

        public int CurrentLives => currentLives;
        public int RequiredBlueTiles => requiredBlueTiles;
        public float LevelTime => levelTime;
        public bool AllowContinue => allowContinue;
    }
}


// GameMode.cs
using UnityEngine;

namespace InteractiveFloor.Core {
    public abstract class GameMode {
        public int CurrentLevelIndex { get; protected set; }
        public abstract void StartMode();
        public abstract void StopMode();
        public abstract void LoadLevel(int index);
        public abstract void RestartMode();

        /// <summary>
        /// Called when a tile is activated (pressed).
        /// </summary>
        public virtual void OnTileActivated(Tile tile) { }
    }
}


// FloorIsLavaMode.cs
using UnityEngine;
using InteractiveFloor.Core;
using System.Collections;
using System.Collections.Generic;

namespace InteractiveFloor.GameModes {
    public class FloorIsLavaMode : GameMode {
        private List<Tile> blueTiles = new List<Tile>();
        private List<Tile> redTiles = new List<Tile>();
        private List<Tile> greenTiles = new List<Tile>();

        private int remainingBlue;
        private float levelDuration;

        public override void StartMode() {
            InitializeLevel();
        }

        public override void LoadLevel(int index) {
            CurrentLevelIndex = index;
            // TODO: load config (positions, counts) from Resources/Levels
            var config = LevelConfigLoader.Load<FloorIsLavaConfig>(index);
            remainingBlue = config.RequiredBlueTiles;
            levelDuration = config.LevelTime;
            CreateTiles(config);
        }

        private void InitializeLevel() {
            ResetTiles();
            CoroutineRunner.Instance.StartCoroutine(LevelTimer());
        }

        private IEnumerator LevelTimer() {
            float t = levelDuration;
            while (t > 0) {
                yield return new WaitForSeconds(1f);
                t -= 1f;
                UIManager.Instance.UpdateTimer(t);
            }
            OnTimeUp();
        }

        private void OnTimeUp() {
            GameManager.Instance.EndGame();
        }

        private void CreateTiles(FloorIsLavaConfig cfg) {
            blueTiles = TileFactory.CreateTiles(cfg.BluePositions, TileType.Blue);
            redTiles = TileFactory.CreateTiles(cfg.RedPositions, TileType.Red);
            greenTiles = TileFactory.CreateTiles(cfg.GreenPositions, TileType.Green);
        }

        private void ResetTiles() {
            foreach (var tile in blueTiles) tile.Reset();
            foreach (var tile in redTiles) tile.Reset();
            // green tiles remain
        }

        public override void OnTileActivated(Tile tile) {
            if (tile.Type == TileType.Blue) {
                tile.SetInactive();
                remainingBlue--;
                UIManager.Instance.UpdateScore(remainingBlue);
                if (remainingBlue <= 0) NextLevel();
            }
            else if (tile.Type == TileType.Red) {
                var gm = GameManager.Instance;
                gm.SetLives(gm.CurrentLives - 1);
                if (gm.CurrentLives <= 0) {
                    if (gm.AllowContinue)
                        RestartMode();
                    else
                        gm.EndGame();
                }
            }
        }

        private void NextLevel() {
            LoadLevel(CurrentLevelIndex + 1);
            InitializeLevel();
        }

        public override void RestartMode() {
            LoadLevel(CurrentLevelIndex);
            InitializeLevel();
        }

        public override void StopMode() {
            // TODO: stop coroutines, clear state
        }
    }
}


// Tile.cs
using UnityEngine;

public enum TileType { Green, Red, Blue }

public class Tile : MonoBehaviour {
    public Vector2Int Position { get; private set; }
    public TileType Type { get; private set; }
    public bool IsFaulty { get; set; }

    private Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    public void Initialize(Vector2Int pos, TileType type) {
        Position = pos;
        Type = type;
        UpdateColor();
    }

    private void UpdateColor() {
        switch (Type) {
            case TileType.Green: rend.material.color = Color.green; break;
            case TileType.Red: rend.material.color = Color.red; break;
            case TileType.Blue: rend.material.color = Color.blue; break;
        }
    }

    private void OnMouseDown() {
        if (IsFaulty) return;
        GameManager.Instance.CurrentMode.OnTileActivated(this);
    }

    public void Reset() {
        UpdateColor();
    }

    public void SetInactive() {
        rend.material.color = Color.gray;
    }
}


// Level.cs
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFloor.Core {
    [System.Serializable]
    public class LevelConfig {
        public int levelIndex;
        public Vector2Int gridSize;
        public List<Vector2Int> greenPositions;
        public List<Vector2Int> redPositions;
        public List<Vector2Int> bluePositions;
        public float levelTime;
        public int requiredBlueTiles;
        // Additional parameters for other modes can be added here
    }

    public class Level : MonoBehaviour {
        public LevelConfig Config { get; private set; }
        private Tile[,] tiles;

        /// <summary>
        /// Инициализирует уровень по конфигурации.
        /// </summary>
        public void Initialize(LevelConfig config) {
            Config = config;
            CreateGrid(config.gridSize);
            SetupTiles();
        }

        private void CreateGrid(Vector2Int gridSize) {
            tiles = new Tile[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    GameObject tileObj = Instantiate(TileFactory.TilePrefab,
                        new Vector3(x, 0, y), Quaternion.identity);
                    Tile tile = tileObj.GetComponent<Tile>();
                    tile.Initialize(new Vector2Int(x, y), TileType.Green);
                    tiles[x, y] = tile;
                }
            }
        }

        private void SetupTiles() {
            // Расставляем плитки согласно конфигу
            foreach (var pos in Config.greenPositions)
                tiles[pos.x, pos.y].Initialize(pos, TileType.Green);
            foreach (var pos in Config.redPositions)
                tiles[pos.x, pos.y].Initialize(pos, TileType.Red);
            foreach (var pos in Config.bluePositions)
                tiles[pos.x, pos.y].Initialize(pos, TileType.Blue);
        }

        /// <summary>
        /// Возвращает плитку по координатам или null, если вне границ.
        /// </summary>
        public Tile GetTile(Vector2Int position) {
            if (position.x < 0 || position.x >= tiles.GetLength(0) ||
                position.y < 0 || position.y >= tiles.GetLength(1))
                return null;
            return tiles[position.x, position.y];
        }

        /// <summary>
        /// Перечисление всех плиток на уровне.
        /// </summary>
        public IEnumerable<Tile> GetAllTiles() {
            foreach (var tile in tiles)
                yield return tile;
        }
    }
}
