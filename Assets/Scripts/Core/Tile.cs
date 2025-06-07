using System;
using UnityEngine;

namespace InteractiveFloor.Core
{
    [Serializable]
    public class Tile
    {
        public Vector2Int Position;
        public TileType Type;
        public Color CurrentColor;
        public bool IsActive;
        public float LastActivatedTime;
        public TileStatus Status;
        public int SegmentId;
        public string Id;
        public bool IsFunctional;
        public int ActivationCount;
        public string HardwareAddress;
        public float PressureValue;
        public TileUsageStats UsageStats;
        public AudioClip SoundEffect;
        public string Note;

        public Tile(Vector2Int position, TileType type, int segmentId, string id = "", string hardwareAddress = "")
        {
            Position = position;
            Type = type;
            CurrentColor = Color.white;
            IsActive = false;
            LastActivatedTime = 0f;
            Status = TileStatus.Idle;
            SegmentId = segmentId;
            Id = id;
            IsFunctional = true;
            ActivationCount = 0;
            HardwareAddress = hardwareAddress;
            PressureValue = 0f;
            UsageStats = new TileUsageStats();
            SoundEffect = null;
            Note = string.Empty;
        }
        public void Activate(float pressure = 1f)
        {
            IsActive = true;
            LastActivatedTime = Time.time;
            Status = TileStatus.Active;
            ActivationCount++;
            PressureValue = pressure;
            UsageStats.TimesActivated++;
        }
        public void Deactivate()
        {
            IsActive = false;
            Status = TileStatus.Idle;
            PressureValue = 0f;
        }
        public void SetFunctional(bool value)
        {
            IsFunctional = value;
        }
        public void SetColor(Color color)
        {
            CurrentColor = color;
        }
        public void AddNote(string note)
        {
            Note = note;
        }
        public void ResetStats()
        {
            UsageStats = new TileUsageStats();
            ActivationCount = 0;
        }
    }

    [Serializable]
    public class TileUsageStats
    {
        public int TimesActivated;
        public float TotalActiveTime;
        public float LastResetTime;

        public TileUsageStats()
        {
            TimesActivated = 0;
            TotalActiveTime = 0f;
            LastResetTime = Time.time;
        }
    }

    public enum TileType
    {
        Default,
        Start,
        Finish,
        Obstacle,
        Special,
        Disabled
    }

    public enum TileStatus
    {
        Idle,
        Active,
        Pressed,
        Error,
        Disabled
    }
}
