using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiGame1 : MonoBehaviour
    {
        [SerializeField] private GameObject tableControl;
        [SerializeField] private TextMeshProUGUI finishText;
        [SerializeField] private TextMeshPro scoreText;
        public Button dropButton;
        public UiWinLose uiWinLose;
        [Header("Load bar")] 
         public Image starBar;
        [SerializeField] private Image yellowBar;
        [SerializeField] private GameObject loadBar,mask;
        public static LogicUiGame1 instance;

        private void Awake()
        {
            instance = this;
            dropButton.onClick.AddListener(OnclickDropButton);
        }

        private void Start()
        {
           // SetUpLoadBar();
        }

        private void OnclickDropButton()
        {
            if (ShadowControl.ins.isCanMove)
            {
                dropButton.GetComponent<Image>().raycastTarget = false;
                Observer.Notify(EventAction.EVENT_DROPBUTTON_CLICKED,null);
                dropButton.GetComponent<RectTransform>().DOAnchorPosY(36, 0.1f).OnComplete(() =>
                {
                    dropButton.GetComponent<RectTransform>().DOAnchorPosY(50, 0.1f);
                });
               // ShadowControl.ins.isCanMove = true;
            }
        }

        public void ShowWinPanel()
        {
            HideTableControl();
            uiWinLose.ShowWin3Star();
            int coinGet = LogicGame1.instance.itemGame1Done.Count * 10;
            Observer.Notify(EventAction.EVENT_GET_COINS, coinGet);
        }
        public void ShowLosePanel()
        {
            HideTableControl();
            uiWinLose.ShowLosePanel();
        }
        private void ShowTableControl()
        {
            tableControl.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0);
            tableControl.transform.DOScale(1, 0.3f).From(0);
        }
        private void HideTableControl()
        {
            tableControl.GetComponent<CanvasGroup>().DOFade(0, 0.3f).From(1);
            tableControl.transform.DOScale(0, 0.3f).From(1);
        }
        public void ScoreRunning(float score)
        {
            scoreText.text = Mathf.RoundToInt(score).ToString() + "%";
        }

        public void ShowScoreText()
        {
            finishText.gameObject.SetActive(true);
            finishText.rectTransform.DOScale(1, 0.3f).From(4).SetEase(Ease.OutBack).OnComplete(() =>
            {
                finishText.DOFade(0, 0.3f).From(1).SetDelay(0.5f).SetEase(Ease.InBack);
                scoreText.gameObject.SetActive(true);
            });
        }

        public void SetPosStarBar(float value)
        {
            /*yellowBar.fillAmount += value;
            Debug.Log(- width / 2 + width * yellowBar.fillAmount );
            starBar.rectTransform.anchoredPosition = new Vector2(- width / 2 + width * yellowBar.fillAmount, 0);*/
            
            float width = yellowBar.rectTransform.rect.width;
            
            yellowBar.DOFillAmount(yellowBar.fillAmount  - 0.03f, 0.5f).OnUpdate(() =>
            {
                starBar.rectTransform.anchoredPosition = new Vector2(-width / 2 + width * yellowBar.fillAmount, 0);
            }).OnComplete(() =>
            {
                yellowBar.DOFillAmount(yellowBar.fillAmount+ value, 0.8f).OnUpdate(() =>
                {
                    starBar.rectTransform.anchoredPosition = new Vector2(-width / 2 + width * yellowBar.fillAmount, 0);
                });
            });
        }
        private void SetUpLoadBar()
        {
            loadBar.transform.position = /*Camera.main.WorldToScreenPoint*/(mask.transform.position -new Vector3(1f,3.2f,0));
        }
    }
}