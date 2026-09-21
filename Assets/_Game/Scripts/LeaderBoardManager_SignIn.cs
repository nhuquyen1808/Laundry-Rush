using IAP_Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class LeaderboardResponse
{
    public bool success;
    public List<LeaderboardItem> data;
    public string timestamp;
}

[Serializable]
public class LeaderboardItem
{
    public int rank;
    public int score;
    public LeaderboardUser user;
    public string period;
    public string periodKey;
}

[Serializable]
public class LeaderboardUser
{
    public string id;
    public string nickname;
    public string avatar;
}

public enum LeaderboardPeriod
{
    ALL_TIME,
    WEEKLY,
    MONTHLY
}

public class LeaderBoardManager_SignIn : Singleton<LeaderBoardManager_SignIn>
{
    public Button closeLeaderBoard;
    public GameObject leaderBoard;
    public Button  leaderBoardBtn;
    public GameObject gobjLeaderBoard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeLeaderBoard.onClick.AddListener(OnClickCloseLeaderBoard);
        leaderBoardBtn.onClick.AddListener(OnClickLeaderBoard);
    }

    private void OnClickLeaderBoard()
    {
        if (PlayerPrefs.GetInt("isLogin") == 1)
        {
            gobjLeaderBoard.gameObject.SetActive(true);

        }
        else
        {
            Debug.Log("Please login to view the leaderboard.");
            SignIn_Panel.Instance.loginPopup.SetActive(true);
        }
    }

    private void OnClickCloseLeaderBoard()
    {
       
        leaderBoard.SetActive(false);
    }

    [SerializeField]
    private string baseUrl = "";

    public IEnumerator GetLeaderboard(
        string gameCode,
        LeaderboardPeriod period,
        int limit,
        Action<List<LeaderboardItem>> onSuccess,
        Action<string> onError)
    {
        if (string.IsNullOrWhiteSpace(gameCode))
        {
            onError?.Invoke("Game code không được để trống.");
            yield break;
        }

        limit = Mathf.Clamp(limit, 1, 100);

        string url =
            $"{baseUrl}/leaderboard" +
            $"?gameCode={UnityWebRequest.EscapeURL(gameCode.Trim())}" +
            $"&period={period}" +
            $"&limit={limit}";

        Debug.Log($"Leaderboard URL: {url}");

        using UnityWebRequest request =
            UnityWebRequest.Get(url);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Accept",
            "application/json"
        );

        yield return request.SendWebRequest();

        string responseText =
            request.downloadHandler.text;

        Debug.Log(
            $"Leaderboard response: {responseText}"
        );

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(
                ParseError(
                    responseText,
                    request.responseCode
                )
            );

            yield break;
        }

        LeaderboardResponse response;

        try
        {
            response =
                JsonUtility.FromJson<LeaderboardResponse>(
                    responseText
                );
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            onError?.Invoke(
                "Không thể đọc dữ liệu bảng xếp hạng."
            );

            yield break;
        }

        if (response == null)
        {
            onError?.Invoke(
                "Máy chủ không trả về dữ liệu hợp lệ."
            );

            yield break;
        }

        if (!response.success)
        {
            onError?.Invoke(
                "Lấy bảng xếp hạng thất bại."
            );

            yield break;
        }

        onSuccess?.Invoke(
            response.data ?? new List<LeaderboardItem>()
        );
    }

    private string ParseError(
        string responseText,
        long responseCode)
    {
        if (!string.IsNullOrWhiteSpace(responseText))
        {
            try
            {
                ApiErrorResponse error =
                    JsonUtility.FromJson<ApiErrorResponse>(
                        responseText
                    );

                if (error != null &&
                    !string.IsNullOrWhiteSpace(error.message))
                {
                    return error.message;
                }
            }
            catch
            {
                // Dùng lỗi mặc định bên dưới.
            }
        }

        return
            $"Không thể tải bảng xếp hạng. HTTP {responseCode}.";
    }

    [Header("API")]
  //  [SerializeField]
    //private LeaderboardApiService leaderboardApiService;

    [Header("Query")]
    [SerializeField] private string gameCode = "Sweet Sort";
    [SerializeField]
    private LeaderboardPeriod period =
        LeaderboardPeriod.ALL_TIME;

    [SerializeField]
    private int limit = 100;

    [Header("List")]
    [SerializeField] private Transform content;
    [SerializeField]
    private LeaderboardItemView itemPrefab;

    [Header("UI")]
    [SerializeField] private TMP_Text messageText;

    private readonly List<LeaderboardItemView>
        spawnedItems = new();

    private bool isLoading;

    private void OnEnable()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        if (isLoading)
            return;

        isLoading = true;
        messageText.text = "Đang tải bảng xếp hạng...";

        StartCoroutine(
            GetLeaderboard(
                gameCode,
                period,
                limit,
                OnLoadSuccess,
                OnLoadFailed
            )
        );
    }

    private void OnLoadSuccess(
        List<LeaderboardItem> items)
    {
        isLoading = false;

        ClearList();

        if (items == null || items.Count == 0)
        {
            messageText.text =
                "Chưa có dữ liệu bảng xếp hạng.";
            return;
        }

        foreach (LeaderboardItem item in items)
        {
            LeaderboardItemView itemView =
                Instantiate(itemPrefab, content);

            itemView.Setup(item);
            spawnedItems.Add(itemView);
        }

        messageText.text = string.Empty;
    }

    private void OnLoadFailed(string error)
    {
        isLoading = false;
        messageText.text = error;

        Debug.LogError(
            $"Load leaderboard failed: {error}"
        );
    }

    private void ClearList()
    {
        foreach (LeaderboardItemView item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        spawnedItems.Clear();
    }
}
