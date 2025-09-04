using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class CurrencyBar : MonoBehaviour
    {
        public static CurrencyBar instance;
        public TextMeshProUGUI coinText;
        public float coin;
        public Button addCoinButton;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            coin = PlayerPrefs.GetFloat(PlayerPrefsManager.Coin);
            coinText.text = coin.ToString();
            addCoinButton.onClick.AddListener(OnclickAddCoinButton);
            Observer.AddObserver(EventAction.EVENT_UPDATE_COIN,UpdateCoinBar);
        }

        private void OnDestroy()
        {
            Observer.RemoveObserver(EventAction.EVENT_UPDATE_COIN,UpdateCoinBar);
        }

        private void UpdateCoinBar(object obj)
        {
           float coinReuduce = (float)obj;
           coin-=coinReuduce;
           coinText.text = coin.ToString();
           coinText.transform.DOKill();
           coinText.transform.DOScale(1.1f, .1f).OnComplete(()=>coinText.transform.DOScale(1f, .1f));
           PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);
        }

        private void OnclickAddCoinButton()
        {
            Debug.Log("Handle add coin when button add coin clicked");
        }

        public void AddCoin(int amount)
        {
            coin += amount;
            coinText.text = coin.ToString();
            coinText.transform.DOKill();
            coinText.transform.DOScale(1.1f, .1f).OnComplete(()=>coinText.transform.DOScale(1f, .1f));
            PlayerPrefs.SetFloat(PlayerPrefsManager.Coin, coin);

        }
      
    }
}