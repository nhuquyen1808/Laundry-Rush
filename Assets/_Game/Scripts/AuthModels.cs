using System;

[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}
[Serializable]
public class LogoutRequest
{
    public string refreshToken;
}
[System.Serializable]
public class RegisterRequest
{
    public string email;
    public string nickname;
    public string password;
}

[Serializable]
public class UserDataOrigin
{
    public int id;
    public string email;
    public string nickname;
}
[Serializable]
public class AuthResponse
{

    public string accessToken;
    public string refreshToken;
    public bool success;
    public string message;
    public AuthData data;
    public string timestamp;

    public string GetAccessToken()
    {
        return data?.accessToken;
    }

    public string GetRefreshToken()
    {
        return data?.refreshToken;
    }
}

[Serializable]
public class AuthData
{
    public string accessToken;
    public string refreshToken;
    public string expiresIn;
}

[Serializable]
public class ApiErrorResponse
{
    public string message;
    public string error;
}