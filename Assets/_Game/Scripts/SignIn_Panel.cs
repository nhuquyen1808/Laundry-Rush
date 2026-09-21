using IAP_Dev;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]

public class SignIn_Panel : Singleton<SignIn_Panel>
{
    //public Button signInBtn, signUpBtn;
    //bool isAccountCorrect, isAccountValid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*signInBtn.onClick.AddListener(OnSignIn);
        signUpBtn.onClick.AddListener(OnSignUp);*/
    }

    /*    private void OnSignIn()
        {
            //Post Api Check username, password correct or incorrect here

            if (isAccountCorrect)
            {

            }
            else
            {

            }
        }

        private void OnSignUp()
        {
            //Post Api Check username, password valid or invalid here

            if (isAccountValid)
            {

            }
            else
            {

            }
        }*/
    [Header("Input")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("UI")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;

    [Header("Service")]
    [SerializeField] private AuthApiService authApiService;

    /*[Header("Scene")]
    [SerializeField] private string mainSceneName = "NewGamePlay";*/

    private bool isRequesting;
    public GameObject loginPopup;

    public Button wantSignUp;


    private void Awake()
    {
        emailInput.contentType =
            TMP_InputField.ContentType.EmailAddress;

        passwordInput.contentType =
            TMP_InputField.ContentType.Password;

        emailInput.ForceLabelUpdate();
        nicknameInput.ForceLabelUpdate();
        passwordInput.ForceLabelUpdate();

        signInButton.onClick.AddListener(OnSignInClicked);
        signUpButton.onClick.AddListener(OnSignUpClicked);
        wantSignUp.onClick.AddListener(OnWantSignUpClicked);

        messageText.text = string.Empty;
    }

    private void OnDestroy()
    {
        signInButton.onClick.RemoveListener(OnSignInClicked);
        signUpButton.onClick.RemoveListener(OnSignUpClicked);
        wantSignUp.onClick.RemoveListener(OnWantSignUpClicked);
    }

    private void OnWantSignUpClicked()
    {
        signInButton.gameObject.SetActive(false);
        signUpButton.gameObject.SetActive(true);
    }

    private void OnSignInClicked()
    {
        if (isRequesting)
            return;

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (!ValidateLoginInput(email, password))
            return;

        SetLoading(true, "Đang đăng nhập...");

        StartCoroutine(
            authApiService.Login(
                email,
                password,
                OnLoginSuccess,
                OnRequestFailed
            )
        );
    }

    private void OnSignUpClicked()
    {
        if (isRequesting)
            return;

        string email = emailInput.text.Trim();
        string nickname = nicknameInput.text.Trim();
        string password = passwordInput.text;

        if (!ValidateRegisterInput(
                email,
                nickname,
                password))
        {
            return;
        }

        SetLoading(true, "Đang đăng ký...");

        StartCoroutine(
            authApiService.Register(
                email,
                nickname,
                password,
                OnRegisterSuccess,
                OnRequestFailed
            )
        );
    }

    private bool ValidateLoginInput(
     string email,
     string password)
    {
        if (!AuthValidator.ValidateEmail(
                email,
                out string emailError))
        {
            FocusInput(emailInput, emailError);
            return false;
        }

        if (!AuthValidator.ValidateLoginPassword(
                password,
                out string passwordError))
        {
            FocusInput(passwordInput, passwordError);
            return false;
        }

        ShowMessage(string.Empty);
        return true;
    }

    private bool ValidateRegisterInput(
        string email,
        string nickname,
        string password)
    {
        if (!AuthValidator.ValidateEmail(
                email,
                out string emailError))
        {
            FocusInput(emailInput, emailError);
            return false;
        }

        if (!AuthValidator.ValidateNickname(
                nickname,
                out string nicknameError))
        {
            FocusInput(nicknameInput, nicknameError);
            return false;
        }

        if (!AuthValidator.ValidateRegisterPassword(
                password,
                out string passwordError))
        {
            FocusInput(passwordInput, passwordError);
            return false;
        }

        ShowMessage(string.Empty);
        return true;
    }

    private void OnLoginSuccess(AuthResponse response)
    {
        SetLoading(false);

        string accessToken = response.GetAccessToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            ShowMessage(
                "Đăng nhập thành công nhưng máy chủ không trả về token."
            );
            return;
        }

        SaveAuthData(response);

        ShowMessage("Đăng nhập thành công.");

       // UiMusicController.Instance.logOutBtn.gameObject.SetActive(true);
        //  SceneManager.LoadScene("NewGamePlay");
        loginPopup.SetActive(false);
        ScoreApiService.Instance.SubmitGameResult("Sweet Sort",
     PlayerPrefs.GetInt("CurrentLevel"),
     PlayerPrefs.GetInt("coin"),
     10, 10,
     new ScoreMetadata { duration = 100, enemyKilled = 10 });
        Debug.Log("Show leaderboard ");
        PlayerPrefs.SetInt("isLogin", 1);
    }

    private void OnRegisterSuccess(AuthResponse response)
    {
        SetLoading(false);

        string accessToken = response.GetAccessToken();

        // Nếu backend trả token ngay sau khi đăng ký.
        if (!string.IsNullOrEmpty(accessToken))
        {
            SaveAuthData(response);
            // SceneManager.LoadScene("NewGamePlay");
            signInButton.gameObject.SetActive(true);
            signUpButton.gameObject.SetActive(false);

            return;
        }

        ShowMessage(
            "Đăng ký thành công. Bạn có thể đăng nhập."
        );

        nicknameInput.text = string.Empty;
        passwordInput.text = string.Empty;

        passwordInput.Select();
    }

    private void OnRequestFailed(string errorMessage)
    {
        SetLoading(false);
        ShowMessage(errorMessage);
    }

    private void SaveAuthData(AuthResponse response)
    {
        string accessToken = response.GetAccessToken();
        string refreshToken = response.GetRefreshToken();

        if (!string.IsNullOrEmpty(accessToken))
        {
            PlayerPrefs.SetString(
                "AccessToken",
                accessToken
            );
        }

        if (!string.IsNullOrEmpty(refreshToken))
        {
            PlayerPrefs.SetString(
                "RefreshToken",
                refreshToken
            );
        }

        PlayerPrefs.Save();
    }

    private void SetLoading(
        bool loading,
        string message = "")
    {
        isRequesting = loading;

        signInButton.interactable = !loading;
        signUpButton.interactable = !loading;

        emailInput.interactable = !loading;
        nicknameInput.interactable = !loading;
        passwordInput.interactable = !loading;

        if (!string.IsNullOrEmpty(message))
        {
            ShowMessage(message);
        }
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
    }

    private void FocusInput(
    TMP_InputField inputField,
    string errorMessage)
    {
        ShowMessage(errorMessage);

        inputField.Select();
        inputField.ActivateInputField();
    }
}
