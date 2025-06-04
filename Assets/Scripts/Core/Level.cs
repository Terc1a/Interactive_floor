public class Level
{
    public List<Segment> Segments { get; private set; }
    public string LevelName { get; private set; }

    public Level(string levelName, List<Segment> segments)
    {
        LevelName = levelName;
        Segments = segments;
    }

    public void StartLevel() { }
    public void EndLevel() { }
}