public class Tile : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    public bool IsActive { get; private set; }

    public void Initialize(Vector2Int position)
    {
        Position = position;
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void OnMouseDown()
    {
        Activate();
        SignalManager.Instance.SendSignal(new Signal(Position));
    }
}