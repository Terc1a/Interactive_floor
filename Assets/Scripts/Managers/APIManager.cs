public class APIManager : MonoBehaviour
{
    private const string ApiUrl = "http://192.168.31.225:8000/books";

    public IEnumerator GetBooks()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(ApiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Handle success
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
            }
        }
    }
}