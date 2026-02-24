using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : Singleton<CoinBar>
{
    public Text coinText;
    public static CoinBar instance;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        var coin = PlayerPrefs.GetInt("coin");
        coinText.text = coin.ToString();
    }
}
