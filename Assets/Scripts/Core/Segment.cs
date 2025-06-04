public class Segment
{
    public List<Tile> Tiles { get; private set; }

    public Segment(List<Tile> tiles)
    {
        Tiles = tiles;
    }

    public void ActivateSegment()
    {
        foreach (var tile in Tiles)
        {
            tile.Activate();
        }
    }

    public void DeactivateSegment()
    {
        foreach (var tile in Tiles)
        {
            tile.Deactivate();
        }
    }
}