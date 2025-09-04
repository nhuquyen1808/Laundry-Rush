using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public Button CoinButton;
    public TextMeshProUGUI coinText;
    public static CoinBar instance;

    private void Awake()
    {
        instance = this;
        CoinButton.onClick.AddListener(OnClickCoinButton);
        UpdateUI();
    }

    private void OnClickCoinButton()
    {
        PopupShop.instance.Show();
    }


    public void UpdateUI()
    {
        int coin = PlayerPrefs.GetInt("coin");
        
        coinText.text = coin.ToString();
    }
   
}
