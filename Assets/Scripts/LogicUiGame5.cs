using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class LogicUiGame5 : MonoBehaviour
    {
        [SerializeField] List<Image> heartPieces = new List<Image>();
        [SerializeField] private TextMeshProUGUI finishText;
        //  [SerializeField] private Image tickImage;
        public static LogicUiGame5 Instance;
        //  public Sprite tickSprite;

        public GameObject tickObject;
        public UiWinLose UiWinLose;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PopupHeartBar();
        }

        private void PopupHeartBar()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < heartPieces.Count; i++)
            {
                /*var a = i;*/
                sequence.Append(heartPieces[i].transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(0));
                sequence.AppendInterval(i * 0.1f);
            }
        }

        public void DisableHeartPieces(int piecesToDisable)
        {
            heartPieces[piecesToDisable].GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetEase(Ease.InBack).OnComplete(
                () => { LogicGame5.ins.PlayNegativeParticleEffect(new Vector3(1, 4, 0)); });
        }

        public void ShowWinUi()
        {
            LogicGame5.ins.isCanPlay = false;
            finishText.gameObject.SetActive(true);
            finishText.transform.DOScale(1, 0.3f).SetDelay(0.4f).From(0).SetEase(Ease.OutBack)
                .OnComplete(() => { UiWinLose.ShowWin3Star(); });
        }

        public void ShowLoseUi()
        {
            LogicGame5.ins.isCanPlay = false;
            finishText.gameObject.SetActive(true);
            finishText.transform.DOScale(1, 0.3f).SetDelay(0.4f).From(0).SetEase(Ease.OutBack)
                .OnComplete(() => { UiWinLose.ShowLosePanel(); });
        }

        public void ShowTickImage(GameObject target)
        {
            /*tickImage.gameObject.SetActive(true);
            tickImage.rectTransform.position = Camera.main.WorldToScreenPoint(target.transform.position);
            tickImage.color = Color.white;
            tickImage.DOFillAmount(1, 0.7f).From(0).SetEase(Ease.OutBack).OnComplete(() =>
            {
                tickImage.DOFade(0, 0.01f).SetEase(Ease.InBack).SetDelay(0.3f);
            });*/
        }
    }
}