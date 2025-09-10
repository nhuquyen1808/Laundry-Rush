using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public Button CoinButton;
    public TextMeshProUGUI coinText;
    public static CoinBar instance;
    public PopupShop popupShop;

    private void Awake()
    {
        instance = this;
        CoinButton.onClick.RemoveAllListeners();
        CoinButton.onClick.AddListener(OnClickCoinButton);
        UpdateUI();
    }

    private void OnDestroy()
    {
        CoinButton.onClick.RemoveAllListeners();
    }

    private void OnClickCoinButton()
    {
        Debug.Log(PopupShop.instance);
        popupShop.Show();
    }


    public void UpdateUI()
    {
        int coin = PlayerPrefs.GetInt("coin");
        coinText.text = coin.ToString();
    }
}
