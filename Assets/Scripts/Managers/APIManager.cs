using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace InteractiveFloor.Core
{
    public class APIManager : MonoBehaviour
    {
        [Header("API Settings")]
        public string BaseUrl = "http://192.168.31.225:8000";
        public float Timeout = 5f;

        public static APIManager Instance { get; private set; }

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

        public void Get(string endpoint, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(GetRequestCoroutine(endpoint, onSuccess, onError));
        }
        public void Post(string endpoint, string jsonData, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(PostRequestCoroutine(endpoint, jsonData, onSuccess, onError));
        }
        public void Put(string endpoint, string jsonData, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(PutRequestCoroutine(endpoint, jsonData, onSuccess, onError));
        }
        public void Delete(string endpoint, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(DeleteRequestCoroutine(endpoint, onSuccess, onError));
        }

        private IEnumerator GetRequestCoroutine(string endpoint, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(BaseUrl + endpoint))
            {
                request.timeout = (int)Timeout;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke(request.downloadHandler.text);
                else
                    onError?.Invoke(request.error);
            }
        }

        private IEnumerator PostRequestCoroutine(string endpoint, string jsonData, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = new UnityWebRequest(BaseUrl + endpoint, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = (int)Timeout;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke(request.downloadHandler.text);
                else
                    onError?.Invoke(request.error);
            }
        }

        private IEnumerator PutRequestCoroutine(string endpoint, string jsonData, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(BaseUrl + endpoint, jsonData))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = (int)Timeout;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke(request.downloadHandler.text);
                else
                    onError?.Invoke(request.error);
            }
        }

        private IEnumerator DeleteRequestCoroutine(string endpoint, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(BaseUrl + endpoint))
            {
                request.timeout = (int)Timeout;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke(request.downloadHandler.text);
                else
                    onError?.Invoke(request.error);
            }
        }

        // Utility methods for parsing or posting specific data types can be added here
        public static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}
