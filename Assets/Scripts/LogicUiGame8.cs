using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiGame8 : MonoBehaviour
    {

        public bool isCanInstantiate;
        public TextMeshPro startText;

        public TextMeshProUGUI scoreText, timeCountDownText, targetText;
        public Button frezeeButton;
        public Image timeFrezeed;

        private void Awake()
        {
            frezeeButton.onClick.AddListener(OnClickFrezeeButton);
        }

        private void OnClickFrezeeButton()
        {
            int coin = PlayerPrefs.GetInt("coin");
            if(coin >= 300)
            {
                coin -= 300;
                AudioManager.instance.PlaySound("Booster");

                PlayerPrefs.SetInt("coin" ,coin);
                CoinBar.instance.UpdateUI();
                frezeeButton.enabled = false;
                ManagerGame.TIME_SCALE = 0;
                timeFrezeed.gameObject.SetActive(true);
                DOVirtual.DelayedCall(5, () =>
                {
                    ManagerGame.TIME_SCALE = 1;
                    timeFrezeed.gameObject.SetActive(false);
                    frezeeButton.enabled = true;

                });
            }
            else
            {
                PopupShop.instance.Show();
            }

        }

        public void AnimStartGame()
        {
            Sequence mySequence = DOTween.Sequence();
            startText.gameObject.SetActive(true);
            startText.text = "3";
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).From(0));
            mySequence.Append(startText.transform.DOScale(0f, 0.2f).SetDelay(0.5f).SetEase(Ease.InBack).OnComplete(()=>startText.text = "2"));
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
            mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(()=>
            {
                startText.text = "1";
            }));
            mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
            mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(()=>startText.text = "Start"));
            mySequence.Append(startText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.5f).OnComplete(()=>
            {
                
                startText.DOFade(0, 0.2f).SetDelay(0.5f).OnComplete(()=> isCanInstantiate = true);
            }));

        }

        public void SetScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetTargetText(int target)
        {
            targetText.text = ": "+ target.ToString();
        }
        public void SetTimeText(float timer)
        {
            /*if(timer >60) 
            {
                timeCountDownText.text = "60";
            }
            else*/ if(timer  < 0)
            {
                timeCountDownText.text = "0";
            }
            else timeCountDownText.text = Mathf.RoundToInt(timer) .ToString();
            
        }
    }
}
