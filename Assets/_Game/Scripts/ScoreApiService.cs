using IAP_Dev;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class SubmitScoreResponse
{
    public bool success;
    public SubmitScoreResponseData data;
    public string timestamp;
}

[Serializable]
public class SubmitScoreResponseData
{
    public ScoreRecord score;
    public UserGameStats stats;
}

[Serializable]
public class ScoreRecord
{
    public string id;
    public string gameCode;
    public int score;
    public int coins;
    public int level;
    public int energy;
    public string createdAt;
}

[Serializable]
public class UserGameStats
{
    public string id;
    public string gameCode;
    public int highScore;
    public int totalScore;
    public int totalPlays;
    public int coins;
    public int level;
    public int energy;
}


[Serializable]
public class SubmitScoreRequest
{
    public string gameCode;
    public int score;
    public float coins;
    public int level;
    public int energy;
    public ScoreMetadata metadata;
}

[Serializable]
public class ScoreMetadata
{
    public int duration;
    public int enemyKilled;

}


public class ScoreApiService : Singleton<ScoreApiService>
{
    [SerializeField]
    private string baseUrl =
        "";
    public IEnumerator SubmitScore(
        string gameCode,
        int score,
        float coins,
        int level,
        int energy,
        ScoreMetadata metadata,
        Action<SubmitScoreResponse> onSuccess,
        Action<string> onError)
    {
        if (string.IsNullOrWhiteSpace(gameCode))
        {
            onError?.Invoke("Game code không được để trống.");
            yield break;
        }

        if (score < 0)
        {
            onError?.Invoke("Score không được nhỏ hơn 0.");
            yield break;
        }

        if (level < 1)
        {
            onError?.Invoke("Level phải lớn hơn hoặc bằng 1.");
            yield break;
        }

        if (energy < 0)
        {
            onError?.Invoke("Energy không được nhỏ hơn 0.");
            yield break;
        }

        string accessToken =
            PlayerPrefs.GetString("AccessToken", string.Empty);

        if (string.IsNullOrEmpty(accessToken))
        {
            onError?.Invoke(
                "Không tìm thấy access token. Vui lòng đăng nhập lại."
            );
            yield break;
        }

        var requestData = new SubmitScoreRequest
        {
            gameCode = gameCode.Trim(),
            score = score,
            coins =  coins,
            level = level,
            energy = energy,
            metadata = metadata
        };

        string json = JsonUtility.ToJson(requestData);

        Debug.Log($"Submit score request: {json}");

        using UnityWebRequest request =
            new UnityWebRequest(
                $"{baseUrl}/scores",
                UnityWebRequest.kHttpVerbPOST
            );

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        request.SetRequestHeader(
            "Authorization",
            $"Bearer {accessToken}"
        );

        yield return request.SendWebRequest();

        Debug.Log(
            $"Submit score response: {request.downloadHandler.text}"
        );

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(
                ParseError(
                    request.downloadHandler.text,
                    request.responseCode
                )
            );

            yield break;
        }

        SubmitScoreResponse response =
            JsonUtility.FromJson<SubmitScoreResponse>(
                request.downloadHandler.text
            );

        if (response == null || !response.success)
        {
            onError?.Invoke(
                "Không thể đọc dữ liệu trả về từ máy chủ."
            );

            yield break;
        }

        onSuccess?.Invoke(response);
    }

    private string ParseError(
        string responseText,
        long responseCode)
    {
        if (!string.IsNullOrEmpty(responseText))
        {
            try
            {
                ApiErrorResponse error =
                    JsonUtility.FromJson<ApiErrorResponse>(
                        responseText
                    );

                if (error != null &&
                    !string.IsNullOrEmpty(error.message))
                {
                    return error.message;
                }
            }
            catch
            {
                // Trả về lỗi mặc định bên dưới.
            }
        }

        return $"Gửi điểm thất bại. HTTP {responseCode}.";
    }

    public void SubmitGameResult(
       string gameCode,
       int score,
       float coins = 0,
       int level = 1,
       int energy = 0,
       ScoreMetadata metadata = null)
    {
        StartCoroutine(
            SubmitScore(
                gameCode,
                score,
                coins,
                level,
                energy,
                metadata,
                OnSubmitScoreSuccess,
                OnSubmitScoreFailed
            )
        );
    }


    private void OnSubmitScoreSuccess(SubmitScoreResponse response)
    {
        Debug.Log("Submit score thành công.");

        if (response.data != null)
        {
            Debug.Log($"Score đã lưu: {response.data.score}");
            Debug.Log($"Stats đã cập nhật.");
        }
    }

    private void OnSubmitScoreFailed(string error)
    {
        Debug.LogError($"Submit score thất bại: {error}");
    }
}