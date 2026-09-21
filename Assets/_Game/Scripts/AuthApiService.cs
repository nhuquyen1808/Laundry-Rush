using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthApiService : MonoBehaviour
{
    [SerializeField] private string BaseUrl = "";


    public IEnumerator Login(
    string email,
    string password,
    Action<AuthResponse> onSuccess,
    Action<string> onError)
    {
        var requestData = new LoginRequest
        {
            email = email.Trim(),
            password = password
        };

        yield return SendAuthRequest(
            endpoint: "/auth/login",
            jsonBody: JsonUtility.ToJson(requestData),
            onSuccess: response =>
            {
                if (!string.IsNullOrEmpty(response.accessToken))
                {
                    PlayerPrefs.SetString(
                        "AccessToken",
                        response.accessToken
                    );
                }

                if (!string.IsNullOrEmpty(response.refreshToken))
                {
                    PlayerPrefs.SetString(
                        "RefreshToken",
                        response.refreshToken
                    );
                }

                PlayerPrefs.Save();

                onSuccess?.Invoke(response);
            },
            onError: onError
        );
    }
    public IEnumerator Register(
     string email,
     string nickname,
     string password,
     Action<AuthResponse> onSuccess,
     Action<string> onError)
    {
        var requestData = new RegisterRequest
        {
            email = email.Trim(),
            nickname = nickname.Trim(),
            password = password
        };

        yield return SendAuthRequest(
            endpoint: "/auth/register",
            jsonBody: JsonUtility.ToJson(requestData),
            onSuccess: onSuccess,
            onError: onError
        );
    }


    public IEnumerator Logout(
     Action onSuccess,
     Action<string> onError)
    {
        string accessToken = PlayerPrefs.GetString(
            "AccessToken",
            ""
        );

        string refreshToken = PlayerPrefs.GetString(
            "RefreshToken",
            ""
        );

        if (string.IsNullOrEmpty(refreshToken))
        {
            onError?.Invoke("Không tìm thấy refresh token.");
            yield break;
        }

        string url = BaseUrl + "/auth/logout";

        var requestData = new LogoutRequest
        {
            refreshToken = refreshToken
        };

        string jsonBody = JsonUtility.ToJson(requestData);
        byte[] body = Encoding.UTF8.GetBytes(jsonBody);

        using UnityWebRequest request = new UnityWebRequest(
            url,
            UnityWebRequest.kHttpVerbPOST
        );

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.timeout = 20;

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        request.SetRequestHeader(
            "Accept",
            "application/json"
        );

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.SetRequestHeader(
                "Authorization",
                $"Bearer {accessToken}"
            );
        }

        Debug.Log($"POST: {url}");
        Debug.Log($"Logout body: {jsonBody}");

        yield return request.SendWebRequest();

        string responseBody = request.downloadHandler?.text;

        Debug.Log($"Status code: {request.responseCode}");
        Debug.Log($"Response: {responseBody}");

        if (request.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.DeleteKey("AccessToken");
            PlayerPrefs.DeleteKey("RefreshToken");
            PlayerPrefs.Save();

            onSuccess?.Invoke();

            yield break;
        }

        string errorMessage = GetErrorMessage(
            request.responseCode,
            responseBody,
            request.error
        );

        onError?.Invoke(errorMessage);
    }



    private IEnumerator SendAuthRequest(
        string endpoint,
        string jsonBody,
        Action<AuthResponse> onSuccess,
        Action<string> onError)
    {
        string url = BaseUrl + endpoint;
        byte[] body = Encoding.UTF8.GetBytes(jsonBody);

        using UnityWebRequest request = new UnityWebRequest(
            url,
            UnityWebRequest.kHttpVerbPOST
        );

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.timeout = 20;

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        request.SetRequestHeader(
            "Accept",
            "application/json"
        );

        Debug.Log($"POST: {url}");
        Debug.Log($"Request body: {jsonBody}");

        yield return request.SendWebRequest();

        string responseBody = request.downloadHandler?.text;

        Debug.Log($"Status code: {request.responseCode}");
        Debug.Log($"Response: {responseBody}");

        if (request.result == UnityWebRequest.Result.Success)
        {
            AuthResponse response;

            try
            {
                response = JsonUtility.FromJson<AuthResponse>(
                    responseBody
                );
            }
            catch (Exception exception)
            {
                onError?.Invoke(
                    $"Không thể đọc dữ liệu máy chủ: {exception.Message}"
                );
                yield break;
            }

            if (response == null)
            {
                onError?.Invoke("Máy chủ trả về dữ liệu không hợp lệ.");
                yield break;
            }

            onSuccess?.Invoke(response);
            yield break;
        }

        string errorMessage = GetErrorMessage(
            request.responseCode,
            responseBody,
            request.error
        );

        onError?.Invoke(errorMessage);
    }

    private string GetErrorMessage(
        long statusCode,
        string responseBody,
        string defaultError)
    {
        if (!string.IsNullOrEmpty(responseBody))
        {
            try
            {
                ApiErrorResponse errorResponse =
                    JsonUtility.FromJson<ApiErrorResponse>(responseBody);

                if (!string.IsNullOrEmpty(errorResponse?.message))
                    return errorResponse.message;

                if (!string.IsNullOrEmpty(errorResponse?.error))
                    return errorResponse.error;
            }
            catch
            {
                // Dùng thông báo theo status code bên dưới.
            }
        }

        return statusCode switch
        {
            400 => "Thông tin gửi lên không hợp lệ.",
            401 => "Tên tài khoản hoặc mật khẩu không chính xác.",
            403 => "Tài khoản không có quyền truy cập.",
            404 => "Không tìm thấy API.",
            409 => "Tên tài khoản đã tồn tại.",
            422 => "Dữ liệu đăng ký không hợp lệ.",
            500 => "Máy chủ đang gặp lỗi.",
            _ => $"Không thể kết nối máy chủ: {defaultError}"
        };
    }
}