public class Signal
{
    public Vector2Int TilePosition { get; private set; }

    public Signal(Vector2Int position)
    {
        TilePosition = position;
    }
}