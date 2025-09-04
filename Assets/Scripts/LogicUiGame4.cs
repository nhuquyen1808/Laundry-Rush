using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiGame4 : MonoBehaviour
    {
        public TextMeshProUGUI countDownTxt;
        public TextMeshProUGUI WaveText;
        public TextMeshProUGUI finishText;
        public GameObject loseUi, winUi;
        private void Start()
        {
            Observer.AddObserver(EventAction.EVENT_POPUP_SHOW_LOSE_DONE, CompleteShowPopUpLose);
            Observer.AddObserver(EventAction.EVENT_POPUP_SHOW_WIN_DONE, CompleteShowWinPanel);
           // ShowWinPanel("");
        }
        public void SetCountDownText()
        {
            countDownTxt.text = "3";
            countDownTxt.transform.DOScale(1.3f, 0.5f).From(0).SetEase(Ease.OutBack).OnComplete(() =>
            {
                countDownTxt.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(() =>
                {
                    countDownTxt.text = "2";
                    countDownTxt.transform.DOScale(1.3f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                    {
                        countDownTxt.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(() =>
                        {
                            countDownTxt.text = "1";
                            countDownTxt.transform.DOScale(1.3f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                            {
                                countDownTxt.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(() =>
                                {
                                    countDownTxt.text = "Start!!";
                                    countDownTxt.transform.DOScale(1.3f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                                    {
                                        countDownTxt.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(0.3f).OnComplete(() =>
                                        {
                                            countDownTxt.text = "";
                                            LogicGame4.instance.isTransitionWave = false;
                                            ManagerGame.TIME_SCALE = 1;
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }
        public void ShowLosePanel()
        {
            finishText.gameObject.SetActive(true);
            finishText.transform.DOScale(1,0.3f) .SetDelay(0.4f).From(0).SetEase(Ease.OutBack).OnComplete(()=> { loseUi.SetActive(true); });
        }
        public void ShowWinPanel()
        {
            finishText.gameObject.SetActive(true);
            finishText.transform.DOScale(1,0.3f) .SetDelay(0.4f).From(0).SetEase(Ease.OutBack).OnComplete(()=> { winUi.SetActive(true); });
        }
        private void CompleteShowPopUpLose(object obj)
        {
            Debug.Log("Complete Show popup lose");
        }
        private void CompleteShowWinPanel(object obj)
        {
            Debug.Log("Complete show Win panel");
          /*  DOTween.KillAll();
            ManagerScene.ins.LoadScene("Game8");*/
        }
        public void ShowWaveText(int wave)
        {
            WaveText.transform.DOScale(1.2f, 0.3f).SetDelay(1.5f).OnComplete(() =>
            {
                WaveText.transform.DOScale(1f, 0.3f).OnComplete(() => { WaveText.text = "Wave " + wave.ToString(); });
            });
        }
        
    }

}
