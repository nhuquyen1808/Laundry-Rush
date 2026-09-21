using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public enum PurchasePlatform
{
    IOS,
    ANDROID,
    WEB
}
[Serializable]
public class VerifyPurchaseRequest
{
    public string gameCode;
    public string productId;
    public string transactionId;
    public double price;
    public string currency;
    public string platform;
    public string receiptData;
}
[Serializable]
public class VerifyPurchaseResponse
{
    public bool success;
    public VerifyPurchaseData data;
    public string message;
    public string timestamp;
}

[Serializable]
public class VerifyPurchaseData
{
    public string transactionId;
    public string productId;
    public string status;
}
public class PurchaseApiService : MonoBehaviour
{
    [SerializeField]
    private string baseUrl = "";

    public IEnumerator VerifyPurchase(
        string gameCode,
        string productId,
        string transactionId,
        double price,
        string currency,
        PurchasePlatform platform,
        string receiptData,
        Action<VerifyPurchaseResponse> onSuccess,
        Action<string> onError)
    {
        if (string.IsNullOrWhiteSpace(gameCode))
        {
            onError?.Invoke("Game code không được để trống.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(productId))
        {
            onError?.Invoke("Product ID không được để trống.");
            yield break;
        }

        if (string.IsNullOrWhiteSpace(transactionId))
        {
            onError?.Invoke("Transaction ID không được để trống.");
            yield break;
        }

        if (price < 0)
        {
            onError?.Invoke("Giá sản phẩm không hợp lệ.");
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

        var requestData = new VerifyPurchaseRequest
        {
            gameCode = gameCode.Trim(),
            productId = productId.Trim(),
            transactionId = transactionId.Trim(),
            price = price,
            currency = string.IsNullOrWhiteSpace(currency)
                ? "USD"
                : currency.Trim().ToUpperInvariant(),
            platform = platform.ToString(),
            receiptData = receiptData ?? string.Empty
        };

        string json = JsonUtility.ToJson(requestData);

        Debug.Log($"Verify purchase request: {json}");

        using UnityWebRequest request =
            new UnityWebRequest(
                $"{baseUrl}/purchase/verify",
                UnityWebRequest.kHttpVerbPOST
            );

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.timeout = 30;

        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        request.SetRequestHeader(
            "Accept",
            "application/json"
        );

        request.SetRequestHeader(
            "Authorization",
            $"Bearer {accessToken}"
        );

        yield return request.SendWebRequest();

        string responseText =
            request.downloadHandler?.text ?? string.Empty;

        Debug.Log(
            $"Verify purchase HTTP {request.responseCode}: " +
            responseText
        );

        if (request.result != UnityWebRequest.Result.Success)
        {
            onError?.Invoke(
                ParsePurchaseError(
                    request.responseCode,
                    responseText,
                    request.error
                )
            );

            yield break;
        }

        VerifyPurchaseResponse response;

        try
        {
            response =
                JsonUtility.FromJson<VerifyPurchaseResponse>(
                    responseText
                );
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            onError?.Invoke(
                "Không thể đọc kết quả xác minh giao dịch."
            );

            yield break;
        }

        if (response == null)
        {
            onError?.Invoke(
                "Máy chủ trả về dữ liệu không hợp lệ."
            );

            yield break;
        }

        if (!response.success)
        {
            onError?.Invoke(
                string.IsNullOrEmpty(response.message)
                    ? "Xác minh giao dịch thất bại."
                    : response.message
            );

            yield break;
        }

        onSuccess?.Invoke(response);
    }

    private string ParsePurchaseError(
        long statusCode,
        string responseText,
        string requestError)
    {
        if (!string.IsNullOrWhiteSpace(responseText))
        {
            try
            {
                ApiErrorResponse errorResponse =
                    JsonUtility.FromJson<ApiErrorResponse>(
                        responseText
                    );

                if (errorResponse != null &&
                    !string.IsNullOrWhiteSpace(
                        errorResponse.message))
                {
                    return errorResponse.message;
                }
            }
            catch
            {
                // Dùng thông báo mặc định bên dưới.
            }
        }

        return statusCode switch
        {
            400 => "Thông tin giao dịch không hợp lệ.",
            401 => "Phiên đăng nhập đã hết hạn.",
            403 => "Bạn không có quyền thực hiện giao dịch.",
            404 => "Không tìm thấy sản phẩm hoặc game.",
            409 => "Giao dịch này đã được xử lý.",
            500 => "Máy chủ đang gặp lỗi khi xác minh giao dịch.",
            _ => $"Xác minh giao dịch thất bại: {requestError}"
        };
    }
}