using System.Text.RegularExpressions;

public static class AuthValidator
{
    private const int MinNicknameLength = 3;
    private const int MaxNicknameLength = 20;

    private const int MinPasswordLength = 8;
    private const int MaxPasswordLength = 32;

    private static readonly Regex EmailRegex =
        new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private static readonly Regex NicknameRegex =
        new Regex(@"^[a-zA-Z0-9_.]+$");

    public static bool ValidateEmail(
        string email,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            errorMessage = "Vui lòng nhập email.";
            return false;
        }

        email = email.Trim();

        if (!EmailRegex.IsMatch(email))
        {
            errorMessage = "Email không đúng định dạng.";
            return false;
        }

        return true;
    }

    public static bool ValidateNickname(
        string nickname,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(nickname))
        {
            errorMessage = "Vui lòng nhập nickname.";
            return false;
        }

        nickname = nickname.Trim();

        if (nickname.Length < MinNicknameLength)
        {
            errorMessage =
                $"Nickname phải có ít nhất {MinNicknameLength} ký tự.";
            return false;
        }

        if (nickname.Length > MaxNicknameLength)
        {
            errorMessage =
                $"Nickname không được vượt quá {MaxNicknameLength} ký tự.";
            return false;
        }

        if (!NicknameRegex.IsMatch(nickname))
        {
            errorMessage =
                "Nickname chỉ được chứa chữ, số, dấu chấm và dấu gạch dưới.";
            return false;
        }

        return true;
    }

    public static bool ValidateLoginPassword(
        string password,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrEmpty(password))
        {
            errorMessage = "Vui lòng nhập mật khẩu.";
            return false;
        }

        return true;
    }

    public static bool ValidateRegisterPassword(
        string password,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrEmpty(password))
        {
            errorMessage = "Vui lòng nhập mật khẩu.";
            return false;
        }

        if (password.Length < MinPasswordLength)
        {
            errorMessage =
                $"Mật khẩu phải có ít nhất {MinPasswordLength} ký tự.";
            return false;
        }

        if (password.Length > MaxPasswordLength)
        {
            errorMessage =
                $"Mật khẩu không được vượt quá {MaxPasswordLength} ký tự.";
            return false;
        }

        if (!Regex.IsMatch(password, "[a-z]"))
        {
            errorMessage =
                "Mật khẩu phải có ít nhất một chữ thường.";
            return false;
        }

        if (!Regex.IsMatch(password, "[A-Z]"))
        {
            errorMessage =
                "Mật khẩu phải có ít nhất một chữ hoa.";
            return false;
        }

        if (!Regex.IsMatch(password, @"\d"))
        {
            errorMessage =
                "Mật khẩu phải có ít nhất một chữ số.";
            return false;
        }

        return true;
    }
}