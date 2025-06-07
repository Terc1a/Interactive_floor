using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFloor.Core
{
    [Serializable]
    public class Segment
    {
        public int SegmentId;
        public List<SegmentTile> Tiles;
        public Vector2Int StartPosition;
        public Vector2Int EndPosition;
        public string Name;
        public bool IsActive;
        public SegmentStatus Status;
        public List<SegmentEvent> Events;
        public Color HighlightColor;
        public string AssignedLevelName;
        public DateTime LastInteraction;
        public int ActivationCount;
        public float LastActivationTime;

        public Segment(int segmentId, string name)
        {
            SegmentId = segmentId;
            Name = name;
            Tiles = new List<SegmentTile>();
            StartPosition = Vector2Int.zero;
            EndPosition = Vector2Int.zero;
            IsActive = false;
            Status = SegmentStatus.Idle;
            Events = new List<SegmentEvent>();
            HighlightColor = Color.white;
            AssignedLevelName = string.Empty;
            LastInteraction = DateTime.MinValue;
            ActivationCount = 0;
            LastActivationTime = 0f;
        }

        public void Activate(float activationTime)
        {
            IsActive = true;
            Status = SegmentStatus.Active;
            LastActivationTime = activationTime;
            ActivationCount++;
            LastInteraction = DateTime.Now;
        }
        public void Deactivate()
        {
            IsActive = false;
            Status = SegmentStatus.Idle;
        }
        public void AddTile(SegmentTile tile)
        {
            Tiles.Add(tile);
        }
        public void RemoveTile(SegmentTile tile)
        {
            Tiles.Remove(tile);
        }
        public void AddEvent(SegmentEvent segmentEvent)
        {
            Events.Add(segmentEvent);
        }
        public void ResetTiles()
        {
            foreach (var tile in Tiles)
            {
                tile.Reset();
            }
        }
    }

    [Serializable]
    public class SegmentTile
    {
        public Vector2Int Position;
        public bool IsFunctional;
        public TileStatus Status;
        public Color Color;
        public float LastActivated;

        public SegmentTile(Vector2Int position)
        {
            Position = position;
            IsFunctional = true;
            Status = TileStatus.Idle;
            Color = Color.white;
            LastActivated = 0f;
        }
        public void Activate(float time)
        {
            Status = TileStatus.Active;
            LastActivated = time;
        }
        public void Deactivate()
        {
            Status = TileStatus.Idle;
        }
        public void SetFunctional(bool value)
        {
            IsFunctional = value;
        }
        public void Reset()
        {
            Status = TileStatus.Idle;
            LastActivated = 0f;
        }
    }

    public enum SegmentStatus
    {
        Idle,
        Active,
        Error,
        Maintenance
    }

    public enum TileStatus
    {
        Idle,
        Active,
        Error
    }

    [Serializable]
    public class SegmentEvent
    {
        public float TriggerTime;
        public string EventType;
        public string Payload;

        public SegmentEvent(float triggerTime, string eventType, string payload)
        {
            TriggerTime = triggerTime;
            EventType = eventType;
            Payload = payload;
        }
    }
}
