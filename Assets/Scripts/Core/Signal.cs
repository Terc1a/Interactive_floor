using System;
using UnityEngine;

namespace InteractiveFloor.Core
{
    [Serializable]
    public class Signal
    {
        public Guid SignalId;
        public SignalType Type;
        public int SegmentId;
        public Vector2Int TilePosition;
        public float Timestamp;
        public string Payload;
        public bool IsProcessed;
        public SignalStatus Status;
        public SignalSource Source;
        public string Sender;
        public float Value;
        public string Description;
        public byte[] RawData;

        public Signal(SignalType type, int segmentId, Vector2Int tilePosition, string sender = "", string payload = "", float value = 0f)
        {
            SignalId = Guid.NewGuid();
            Type = type;
            SegmentId = segmentId;
            TilePosition = tilePosition;
            Timestamp = Time.time;
            Payload = payload;
            IsProcessed = false;
            Status = SignalStatus.Received;
            Source = SignalSource.Hardware;
            Sender = sender;
            Value = value;
            Description = string.Empty;
            RawData = null;
        }
        public void MarkProcessed()
        {
            IsProcessed = true;
            Status = SignalStatus.Processed;
        }
        public void SetStatus(SignalStatus status)
        {
            Status = status;
        }
        public void SetSource(SignalSource source)
        {
            Source = source;
        }
    }

    public enum SignalType
    {
        Press,
        Release,
        Hold,
        Error,
        Custom
    }

    public enum SignalStatus
    {
        Received,
        Processed,
        Failed
    }

    public enum SignalSource
    {
        Hardware,
        Software,
        Emulator,
        Manual
    }
}
