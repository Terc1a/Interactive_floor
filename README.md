# Unity Tile Interaction Project

This Unity project implements an interactive floor system using a tile-based structure. The system is designed according to the technical specification and features multiple levels, segments, a tile editor, and integration with an external API.

## 🎮 Features

- 2D tile-based interactive environment
- Segment grouping and control
- Level loading and management
- Basic tile editor UI
- API integration for data retrieval
- Modular architecture for future extensions

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Core/             # Base classes: Tile, Segment, Level, Signal
│   ├── Managers/         # Game, Level and API managers
│   └── UI/               # User interface scripts
├── Prefabs/              # Tile and Segment prefabs (to be added)
├── Scenes/               # MainMenu, LevelEditor, GameScene
└── Resources/
    ├── Tiles/            # Tile assets
    └── Levels/           # Level configurations
```

## 🚀 Getting Started

1. Open Unity and create a new 2D project.
2. Unpack the contents of this repository into the `Assets` directory.
3. Create necessary prefabs for `Tile` and `Segment`.
4. Add your scenes: `MainMenu`, `LevelEditor`, `GameScene`.
5. Setup UnityEvent triggers for tile clicks and UI buttons.
6. Assign scripts to appropriate GameObjects.

## 🌐 API Integration

APIManager includes an example GET request to an endpoint:

```
http://192.168.31.225:8000/books
```

Update the endpoint and response handling logic according to your real API.

## 🛠️ Requirements

- Unity 2021 or newer
- .NET 4.x scripting runtime
- Internet access for API features

## 📌 Notes

- This is a scaffold project based on a technical specification.
- You are expected to add Unity UI elements, prefabs, and concrete game logic.
- Use `GameManager`, `APIManager`, and `UIManager` as central control points.

## 📃 License

This project is provided "as-is" under an open-source license for educational and prototyping use.

---

Created by AI based on your provided technical specification.