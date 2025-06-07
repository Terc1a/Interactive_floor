using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFloor.Core
{
    [Serializable]
    public class Level
    {
        public string LevelName;
        public string Description;
        public int Difficulty;
        public List<LevelTile> Tiles;
        public List<LevelGoal> Goals;
        public float TimeLimit;
        public Sprite PreviewImage;
        public AudioClip BackgroundMusic;
        public bool IsLocked;
        public int HighScore;
        public int MaxCombo;
        public DateTime LastPlayed;
        public List<LevelEvent> Events;

        public Level()
        {
            LevelName = "New Level";
            Description = string.Empty;
            Difficulty = 1;
            Tiles = new List<LevelTile>();
            Goals = new List<LevelGoal>();
            TimeLimit = 0f;
            PreviewImage = null;
            BackgroundMusic = null;
            IsLocked = true;
            HighScore = 0;
            MaxCombo = 0;
            LastPlayed = DateTime.MinValue;
            Events = new List<LevelEvent>();
        }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
        public void SetHighScore(int score)
        {
            if (score > HighScore)
                HighScore = score;
        }
        public void SetMaxCombo(int combo)
        {
            if (combo > MaxCombo)
                MaxCombo = combo;
        }
        public void UpdateLastPlayed()
        {
            LastPlayed = DateTime.Now;
        }
        public void AddTile(LevelTile tile)
        {
            Tiles.Add(tile);
        }
        public void RemoveTile(LevelTile tile)
        {
            Tiles.Remove(tile);
        }
        public void AddGoal(LevelGoal goal)
        {
            Goals.Add(goal);
        }
        public void AddEvent(LevelEvent levelEvent)
        {
            Events.Add(levelEvent);
        }
        public void ResetProgress()
        {
            foreach (var goal in Goals)
                goal.IsCompleted = false;
            MaxCombo = 0;
        }
    }

    [Serializable]
    public class LevelTile
    {
        public Vector2Int Position;
        public string Type;
        public Color Color;
        public bool IsActive;
        public float ActivationTime;
        public int ScoreValue;
        public AudioClip SoundEffect;

        public LevelTile(Vector2Int position, string type, Color color, bool isActive = false)
        {
            Position = position;
            Type = type;
            Color = color;
            IsActive = isActive;
            ActivationTime = 0f;
            ScoreValue = 0;
            SoundEffect = null;
        }
        public void Activate(float time)
        {
            IsActive = true;
            ActivationTime = time;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }

    [Serializable]
    public class LevelGoal
    {
        public string GoalType;
        public int TargetValue;
        public bool IsCompleted;
        public string Description;
        public Sprite Icon;

        public LevelGoal(string goalType, int targetValue, string description = "")
        {
            GoalType = goalType;
            TargetValue = targetValue;
            IsCompleted = false;
            Description = description;
            Icon = null;
        }
        public void Complete()
        {
            IsCompleted = true;
        }
        public void Reset()
        {
            IsCompleted = false;
        }
    }

    [Serializable]
    public class LevelEvent
    {
        public float TriggerTime;
        public string EventType;
        public string Payload;

        public LevelEvent(float triggerTime, string eventType, string payload)
        {
            TriggerTime = triggerTime;
            EventType = eventType;
            Payload = payload;
        }
    }
}
