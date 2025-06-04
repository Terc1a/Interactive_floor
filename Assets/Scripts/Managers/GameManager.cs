public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Level CurrentLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string levelName) { }
    public void StartGame() { CurrentLevel.StartLevel(); }
    public void EndGame() { CurrentLevel.EndLevel(); }
}